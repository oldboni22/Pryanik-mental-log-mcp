namespace Domain.Entities;

public sealed class LogEntry : ITimeStamp
{
    public Guid Id { get; init; }
    
    public DateTime TimeStamp { get; set; }
    
    public required string Summary { get; init; }
    
    public required string Text { get; init; }

    public IEnumerable<EntryChunk> Chunks { get; init; } = [];
    
    public IEnumerable<Advice> Advices { get; init; } = [];
}
