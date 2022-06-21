# HyperaiX

下一代 [Hyperai]("https://github.com/theGravityLab/Hyperai")。

```csharp
// SomeUnit.cs
[Receiver(MessageEventType.Group | MessageEventType.Friend)]
[Extract("!reply {image:Image}")]
public async Task ReplyImageAsync(Image image, MessageChain chain)
{
    var builder = chain.CanBeReplied() ? chain.MakeReply() : new MessageChainBuilder();
    builder.Add(image);
    await Context.SendMessageAsync(builder.Build());
}

[Receiver(MessageEventType.Group | MessageEventType.Friend)]
[Extract("{owner}/{repo}")]
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

## TODO

日后实现

### Unit Scope(弃案，Unit不应该用来保存状态)

通过切换 Unit 的上下文实现对于一个 Unit 在同一时间只为一个用户提供服务。也就是说一个或不同的 Unit 中的 UnitContext 会保证在同一个用户下一致。

Unit Context 保存了 Unit 的状态，其中有 Unit 的持久化数据(kv数据库，Unit生命中周期为一次事务)，其他不知道，以后再加。

使用 Attribute 来决定上下文共享方式，例如`[SharingScope(UnitContextScope)]`。其中 `UnitContextScope` 具有以下值：

- 单Unit单用户
- 单Unit多用户
- 多Unit单用户
- 全局共享

### Session(完成)

Unit Service 会提供一个集中保存状态的地方，并用`Session`表示，可以被注入到 Action。

`PersistenceAttribute`标记的 Action 就可以获得的`Session`。
