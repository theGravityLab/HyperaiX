# HyperaiX

下一代 [Hyperai]("https://github.com/theGravityLab/Hyperai")。

```csharp
// SomeUnit.cs
[Receiver(MessageEventType.Group | MessageEventType.Friend)]
[Handler("!reply {image:Image}")]
public async Task ReplyImageAsync(Image image, MessageChain chain)
{
    var builder = chain.CanBeReplied() ? chain.MakeReply() : new MessageChainBuilder();
    builder.Add(image);
    await Context.SendMessageAsync(builder.Build());
}

[Receiver(MessageEventType.Group | MessageEventType.Friend)]
[Handler("{owner}/{repo}")]
public Image Github(string owner, string repo)
{
    return new Image(new Uri($"https://opengraph.githubassets.com/0/{owner}/{repo}"));
}
```

## Install for Bot

```sh
dotnet add package HyperaiX
```
添加到程序通用/Web主机中：
```csharp
host.ConfigureServices( services => services
    .AddHyperaiX()
    .AddSingleton<Bridge>());
// 别忘了实现 IApiClient

class Bridge: IApiClient
{
    // 实例负责接收/发送事件
    // 接收到的事件被发送到 HyperaiX 管线中处理
}

```

## Install for Development

`HyperaiX.Abstractions` 提供了大部分对qq消息处理的基础类型，在需要的地方使用该包。

```sh
dotnet add package HyperaiX.Abstractions
```
需要的场景：
- 框架
- API
- BOT

## Documents
没有。看看[上一代的文档](https://projhyperai.dowob.vip)吧。
