namespace Domain;

public sealed class LogEntry
{
    public Guid Id { get; init; }
    
    public DateTime Timestamp { get; init; }
    
    public required string Summary { get; init; }
    
    public required string Text { get; init; }

    public IEnumerable<EntryChunk> Chunks { get; init; } = [];
}
