using Domain.Interfaces;

namespace Domain.Entities;

public sealed class Trait : ITimeStamp, IId
{
    public Guid Id { get; init; }
    
    public DateTime TimeStamp { get; set; }
    
    public required string Description { get; init; }
    
    public float[] Embedding { get; init; }
    
    public IEnumerable<TraitEntryRelation> TraitRelations { get; init; } = new List<TraitEntryRelation>();
}
