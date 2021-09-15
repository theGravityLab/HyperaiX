using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public abstract record StreamFileBase: MessageElement
    {
        public IFileSource Source { get; protected init; }
    }
}