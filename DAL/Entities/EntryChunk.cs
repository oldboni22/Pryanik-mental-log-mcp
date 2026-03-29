namespace Domain.Entities;

public sealed class EntryChunk
{
    public Guid EntryId { get; init; }

    public int Number { get; init; }
    
    public int TotalChunks { get; init; }
    
    public required string Text { get; init; }

    public required float[] Embedding { get; init; }
    
    public LogEntry Entry { get; init; }
}
