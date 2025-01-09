using HyperaiX.Modules;

namespace HyperaiX.Services;

public class ModuleRegistry
{
    private readonly List<Module> _modules = [];
    public IReadOnlyList<Module> Modules => _modules;

    public void Add(Module module) => _modules.Add(module);

    // TODO: 用 ECS 架构的数据结构来优化一下？
    // 能否在返回的时候携带 IsActive 和 Key，方便 BotMiddleware 去控制 BotFeature 的缓存有效性
    public IReadOnlyList<T> GetFeatures<T>()
    {
        return Modules.Where(x => x.IsActive).SelectMany(x => x.Features).Where(x => x.Key == typeof(T))
            .Select(x => (T)x.Value).ToList();
    }
}