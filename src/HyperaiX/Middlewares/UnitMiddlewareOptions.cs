using System.Reflection;
using HyperaiX.Abstractions.Messages.Payloads.Elements;

namespace HyperaiX.Middlewares;

public class UnitMiddlewareOptions
{
    private List<Type> _elementTypes = new List<Type>();
    public IList<Type> ElementTypes => _elementTypes;

    public void AddElementType<T>() where T : IMessageElement
    {
        _elementTypes.Add(typeof(T));
    }

    public void AddElementType(Assembly assembly)
    {
        foreach (var type in assembly.GetExportedTypes().Where(x => x is
                 {
                     IsAbstract: false, IsInterface: false, IsGenericType: false, IsGenericTypeDefinition: false,
                     IsNested: false, IsClass: true
                 } && x.IsAssignableTo(typeof(IMessageElement))))
        {
            _elementTypes.Add(type);
        }
    }
}