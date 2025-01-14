using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Builders;
using HyperaiX.Extensions.QQ.Messages.Payloads.Elements;
using IBuilder;

namespace HyperaiX.Extensions.QQ.Messages.Builders;

public static class RichContentBuilderExtensions
{
    public static RichContentBuilder At(this RichContentBuilder self, ulong userId, string? display = null)
    {
        return self.AddElement(new At(userId, display ?? userId.ToString()));
    }
}