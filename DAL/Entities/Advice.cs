using Domain.Interfaces;

namespace Domain.Entities;

public sealed class Advice : ITimeStamp, IText, IId, IEmbedding
{
    public Guid Id { get; init; }
    
    public DateTime TimeStamp { get; set; }
    
    public Guid? SourceEntryId { get; init; } 
    
    public Entry? SourceEntry { get; init; }
    
    public required string Topic { get; init; }
    
    public required string Summary { get; init; }

    public required string Text { get; init; }
    
    public int TextLength { get; set; }

    public required float[] Embedding { get; init; }
}
