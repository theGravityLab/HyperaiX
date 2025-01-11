using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Clients.Lagrange.Services;
using HyperaiX.Clients.Lagrange.Utilties;
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
    private readonly ILogger _logger;
    private readonly BotContext _context;
    private readonly MemoryStore _store;

    // 拉格朗日的事件是在 Task.Run 中发起的，发起线程未知，但至少 Writer 有多个且在线程池里
    private readonly Channel<GenericEventArgs> _events =
        Channel.CreateUnbounded<GenericEventArgs>(new UnboundedChannelOptions()
            { SingleReader = true, SingleWriter = false, AllowSynchronousContinuations = false });

    private static readonly string DEVICEINFO_PATH = Path.Combine(Environment.CurrentDirectory, "lagrange.device.json");
    private static readonly string KEYSTORE_PATH = Path.Combine(Environment.CurrentDirectory, "lagrange.keystore.json");
    private static readonly string QRCODE_PATH = Path.Combine(Environment.CurrentDirectory, "lagrange.qrcode.png");

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
            deviceInfo = new BotDeviceInfo()
            {
                Guid = Guid.NewGuid(),
                MacAddress = Enumerable.Range(0, 6).Select(x => (byte)Random.Shared.Next(256)).ToArray(),
                DeviceName = $"OPPO A5",
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
        var friend = _store.GetFriendAsync(e.Chain.FriendUin, _context).GetAwaiter().GetResult();
        var message = ModelHelper.ToMessage(e.Chain);
        var evt = new MessageEventArgs(new Conversation(friend), friend, message);
        _events.Writer.WriteAsync(evt);
    }

    private void InvokerOnOnGroupMessageReceived(BotContext context, GroupMessageEvent e)
    {
        if (e.Chain is { GroupUin: not null })
        {
            var group = _store.GetGroupAsync(e.Chain.GroupUin.Value, context).GetAwaiter().GetResult();
            var member = _store.GetMemberAsync(group.Id, e.Chain.FriendUin, e.Chain.GroupMemberInfo, context)
                .GetAwaiter().GetResult();
            var message = ModelHelper.ToMessage(e.Chain);
            var evt = new MessageEventArgs(group, member, message);
            _events.Writer.WriteAsync(evt);
        }
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
            {
                _logger.LogError("Failed getting logged in silently, please remove keystore file and restart.");
            }
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
        if (await _events.Reader.WaitToReadAsync(token))
        {
            return await _events.Reader.ReadAsync(token);
        }

        throw new OperationCanceledException();
    }

    public ValueTask<GenericReceiptArgs> WriteAsync(GenericActionArgs action, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}