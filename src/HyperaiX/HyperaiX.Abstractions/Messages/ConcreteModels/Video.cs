using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Video: StreamFileBase
    {
        public Video(IFileSource source) => Source = source;
    }
}