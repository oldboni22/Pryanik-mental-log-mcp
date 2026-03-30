namespace Domain.Entities;

public sealed class TraitEntryRelation
{
    public Guid EntryId { get; init; }
    
    public Guid TraitId { get; init; }
}
