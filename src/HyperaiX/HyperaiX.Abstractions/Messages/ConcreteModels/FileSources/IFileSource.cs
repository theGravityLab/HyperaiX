using System.IO;

namespace HyperaiX.Abstractions.Messages.ConcreteModels.FileSources
{
    public interface IFileSource
    {
        Stream OpenRead();
    }
}