using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Audio: StreamFileBase
    {
        public Audio(IFileSource source) => Source = source;
    }
}