namespace Domain;

public sealed class Advice
{
    public Guid Id { get; init; }
    
    public Guid? SourceEntryId { get; init; } 
    
    public LogEntry? SourceEntry { get; init; }
    
    public required string Topic { get; init; }
    
    public required string Summary { get; init; }
    
    public required string Text { get; init; }
}
