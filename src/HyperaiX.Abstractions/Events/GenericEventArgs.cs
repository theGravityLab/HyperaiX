namespace HyperaiX.Abstractions.Events;

public record GenericEventArgs
{
    public DateTimeOffset Time { get; } = DateTimeOffset.UtcNow;
}