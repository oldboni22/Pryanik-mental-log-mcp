using Domain.Interfaces;

namespace Domain.Entities;

public sealed class Entry : ITimeStamp, IText, IId
{
    public Guid Id { get; init; }
    
    public DateTime TimeStamp { get; set; }
    
    public required string Summary { get; init; }
    
    public required string Text { get; init; }
    
    public int TextLength { get; set; }

    public required int TotalChunks { get; init; }
    
    public IEnumerable<EntryChunk> Chunks { get; init; } = new List<EntryChunk>();
    
    public IEnumerable<Advice> Advices { get; init; } = new List<Advice>();
    
    public IEnumerable<TraitEntryRelation> TraitRelations { get; init; } = new List<TraitEntryRelation>();
}
