using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages
{
    public static class MessageChainBuilderExtensions
    {
        public static MessageChainBuilder AddPlain(this MessageChainBuilder builder, string text)
        {
            var plain = new Plain(text);
            return builder.Add(plain);
        }

        public static MessageChainBuilder AddImage(this MessageChainBuilder builder, string imageId,
            IFileSource source)
        {
            var image = new Image(imageId, source);
            return builder.Add(image);
        }

        public static MessageChainBuilder AddFace(this MessageChainBuilder builder, int faceId)
        {
            var face = new Face(faceId);
            return builder.Add(face);
        }

        public static MessageChainBuilder AddFlash(this MessageChainBuilder builder, string imageId,
            IFileSource source)
        {
            var image = new Flash(imageId, source);
            return builder.Add(image);
        }

        public static MessageChainBuilder AddQuote(this MessageChainBuilder builder, long target)
        {
            var quote = new Quote(target);
            return builder.Add(quote);
        }

        public static MessageChainBuilder AddAt(this MessageChainBuilder builder, long who)
        {
            var at = new At(who);
            return builder.Add(at);
        }

        public static MessageChainBuilder AddAtAll(this MessageChainBuilder builder)
        {
            var atAll = new AtAll();
            return builder.Add(atAll);
        }
    }
}