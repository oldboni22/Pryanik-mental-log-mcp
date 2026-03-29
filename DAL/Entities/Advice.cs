namespace Domain.Entities;

public sealed class Advice : ITimeStamp
{
    public Guid Id { get; init; }
    
    public DateTime TimeStamp { get; set; }
    
    public Guid? SourceEntryId { get; init; } 
    
    public LogEntry? SourceEntry { get; init; }
    
    public required string Topic { get; init; }
    
    public required string Summary { get; init; }

    public required string Text { get; init; }
    
    public required float[] Embedding { get; init; }
}
