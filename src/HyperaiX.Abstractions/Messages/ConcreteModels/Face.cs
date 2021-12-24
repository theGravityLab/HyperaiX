namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Face : MessageElement
{
    public Face(int faceId)
    {
        FaceId = faceId;
    }

    public int FaceId { get; init; }
}