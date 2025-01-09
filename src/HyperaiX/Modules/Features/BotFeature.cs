using System.Collections;
using HyperaiX.Abstractions.Bots;

namespace HyperaiX.Modules.Features;

public class BotFeature
{
    internal IList<Type> BotTypes { get; } = new List<Type>();

    // 临时存放，具体使用看 BotMiddleware
    internal IList<BotBase>? ActivatedBots { get; set; }

    public void Add<T>() where T : BotBase
    {
        BotTypes.Add(typeof(T));
    }
}