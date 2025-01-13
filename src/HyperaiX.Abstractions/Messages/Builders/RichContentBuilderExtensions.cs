using HyperaiX.Abstractions.Messages.Payloads.Elements;

namespace HyperaiX.Abstractions.Messages.Builders;

public static class RichContentBuilderExtensions
{
    public static RichContentBuilder Text(this RichContentBuilder self, string text)
    {
        return self.AddElement(new Text(text));
    }
}