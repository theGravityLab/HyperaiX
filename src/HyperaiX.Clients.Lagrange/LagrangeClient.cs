using System.Text.Json;
using System.Threading.Channels;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Clients.Lagrange.Services;
using HyperaiX.Clients.Lagrange.Utilties;
using HyperaiX.Extensions.QQ.Receipts;
using HyperaiX.Extensions.QQ.Roles;
using Lagrange.Core;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Event.EventArg;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyperaiX.Clients.Lagrange;

public class LagrangeClient : IEndClient
{
    private static readonly string DEVICEINFO_PATH = Path.Combine(Environment.CurrentDirectory, "lagrange.device.json");
    private static readonly string KEYSTORE_PATH = Path.Combine(Environment.CurrentDirectory, "lagrange.keystore.json");
    private static readonly string QRCODE_PATH = Path.Combine(Environment.CurrentDirectory, "lagrange.qrcode.png");
    private readonly BotContext _context;

    // 拉格朗日的事件是在 Task.Run 中发起的，发起线程未知，但至少 Writer 有多个且在线程池里
    private readonly Channel<GenericEventArgs> _events =
        Channel.CreateUnbounded<GenericEventArgs>(new UnboundedChannelOptions
            { SingleReader = true, SingleWriter = false, AllowSynchronousContinuations = false });

    private readonly ILogger _logger;
    private readonly MemoryStore _store;

    public LagrangeClient(IOptions<LagrangeClientOptions> options, ILogger<LagrangeClient> logger, MemoryStore store)
    {
        _logger = logger;
        _store = store;

        BotDeviceInfo? deviceInfo = null;

        BotKeystore? keystore = null;

        try
        {
            if (File.Exists(DEVICEINFO_PATH))
                deviceInfo = JsonSerializer.Deserialize<BotDeviceInfo>(File.ReadAllText(DEVICEINFO_PATH));

            if (File.Exists(KEYSTORE_PATH))
                keystore = JsonSerializer.Deserialize<BotKeystore>(File.ReadAllText(KEYSTORE_PATH));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Restore previous bot login status file failed.");
        }

        if (deviceInfo is null)
        {
            deviceInfo = new BotDeviceInfo
            {
                Guid = Guid.NewGuid(),
                MacAddress = Enumerable.Range(0, 6).Select(x => (byte)Random.Shared.Next(256)).ToArray(),
                DeviceName = "OPPO A5",
                KernelVersion = "6.4",
                SystemKernel = "Linux 6.4"
            };
            File.WriteAllText(DEVICEINFO_PATH, JsonSerializer.Serialize(deviceInfo));
        }

        var bot = BotFactory.Create(new BotConfig(), deviceInfo, keystore ?? new BotKeystore());

        bot.Invoker.OnBotLogEvent += InvokerOnOnBotLogEvent;
        bot.Invoker.OnBotCaptchaEvent += InvokerOnOnBotCaptchaEvent;
        bot.Invoker.OnFriendMessageReceived += InvokerOnOnFriendMessageReceived;
        bot.Invoker.OnGroupMessageReceived += InvokerOnOnGroupMessageReceived;

        _context = bot;
    }

    public async Task ConnectAsync(CancellationToken token)
    {
        if (_context.UpdateKeystore().Uin == 0)
        {
            if (await _context.FetchQrCode() is var (_, bytes))
            {
                await File.WriteAllBytesAsync(QRCODE_PATH, bytes,
                    token);
                _logger.LogInformation("QrCode file was saved at {}.", QRCODE_PATH);
                await _context.LoginByQrCode();
                await File.WriteAllTextAsync(KEYSTORE_PATH, JsonSerializer.Serialize(_context.UpdateKeystore()), token);
            }
            else
            {
                _logger.LogError("Failed getting Login QrCode, entering sleeping mode.");
            }
        }
        else
        {
            if (!await _context.LoginByPassword())
                _logger.LogError("Failed getting logged in silently, please remove keystore file and restart.");
        }

        _logger.LogInformation("Logged in as {}", _context.UpdateKeystore().Uin);
    }

    public Task DisconnectAsync(CancellationToken token)
    {
        _events.Writer.Complete();
        _context.Dispose();
        return Task.CompletedTask;
    }

    public async ValueTask<GenericEventArgs> ReadAsync(CancellationToken token)
    {
        if (await _events.Reader.WaitToReadAsync(token)) return await _events.Reader.ReadAsync(token);

        throw new OperationCanceledException();
    }

    public async ValueTask<GenericReceiptArgs> WriteAsync(GenericActionArgs action, CancellationToken token = default)
    {
        if (token.IsCancellationRequested) await ValueTask.FromCanceled<GenericReceiptArgs>(token);
        switch (action)
        {
            case SendMessageActionArgs sendMessage:
            {
                var seq = (await _context.SendMessage(ModelHelper.FromMessage(sendMessage.Chat, sendMessage.Message)))
                    .Sequence;
                if (seq.HasValue)
                    // MessageId 的类型是 Client 的属性，由 Client 确定而非 Extension，这里应该返回更为一般且由 HyperaiX.Abstractions 做去平台化 HashCode 类型的 Handle
                    // 在拉格朗日中 Handle 类型为 ulong，且就是 Sequence，需要在发送时转换为 Sequence，接受时提取自 Sequence 不做类型转换
                    return new SendMessageReceiptArgs(seq.Value);

                throw new NotImplementedException("Should throw or just return ErrorReceiptArgs?");
            }
            default:
                return new GenericReceiptArgs();
        }
    }

    private void InvokerOnOnBotLogEvent(BotContext context, BotLogEvent e)
    {
        _logger.LogDebug("Bot ({}) {}: {}", e.Level, e.Tag, e.EventMessage);
    }

    private void InvokerOnOnBotCaptchaEvent(BotContext context, BotCaptchaEvent e)
    {
        _logger.LogError("Bot has received a captcha request.");
    }

    private void InvokerOnOnFriendMessageReceived(BotContext context, FriendMessageEvent e)
    {
        var friend = _store.GetFriendAsync(e.Chain.FriendUin, e.Chain.FriendInfo, _context).GetAwaiter().GetResult();
        var self = _store.GetFriendAsync(context.BotUin, null, context).GetAwaiter().GetResult();
        var message = ModelHelper.ToMessage(e.Chain);
        var evt = new MessageEventArgs(new Conversation(friend), friend, self, message);
        _events.Writer.WriteAsync(evt);
    }

    private void InvokerOnOnGroupMessageReceived(BotContext context, GroupMessageEvent e)
    {
        if (e.Chain is { GroupUin: not null })
        {
            var group = _store.GetGroupAsync(e.Chain.GroupUin.Value, null, context).GetAwaiter().GetResult();
            var member = _store
                .GetMemberAsync(e.Chain.GroupUin.Value, e.Chain.FriendUin, e.Chain.GroupMemberInfo, context)
                .GetAwaiter().GetResult();
            var self = _store.GetMemberAsync(e.Chain.GroupUin.Value, context.BotUin, null, context).GetAwaiter()
                .GetResult();
            var message = ModelHelper.ToMessage(e.Chain);
            var evt = new MessageEventArgs(group, member, self, message);
            _events.Writer.WriteAsync(evt);
        }
    }
}