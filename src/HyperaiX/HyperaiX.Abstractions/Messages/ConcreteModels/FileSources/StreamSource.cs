using System;
using System.IO;

namespace HyperaiX.Abstractions.Messages.ConcreteModels.FileSources
{
    public class StreamSource
        : IFileSource, IDisposable
    {
        public StreamSource(Stream data)
        {
            Data = data;
        }

        public Stream Data { get; set; }

        public void Dispose()
        {
            Data?.Dispose();
        }

        public Stream OpenRead()
        {
            return Data;
        }
    }
}