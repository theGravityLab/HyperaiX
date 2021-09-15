using System;
using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Image: ImageBase
    {
        public Image(string imageId,IFileSource source)
        {
            ImageId = imageId;
            Source = source;
        }
    }
}