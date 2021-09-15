using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Flash: ImageBase
    {
        public Flash(string imageId, IFileSource source)
        {
            ImageId = imageId;
            Source = source;
        }
    }
}