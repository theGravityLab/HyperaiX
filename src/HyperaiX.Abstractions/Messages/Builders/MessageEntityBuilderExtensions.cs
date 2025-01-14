namespace HyperaiX.Abstractions.Messages.Builders;

public static class MessageEntityBuilderExtensions
{
    public static RichContentBuilder RichContent(this MessageEntityBuilder self)
    {
        var builder = new RichContentBuilder(self);
        return builder;
    }
}