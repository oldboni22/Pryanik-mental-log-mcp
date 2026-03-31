namespace Domain.Entities;

public sealed class TraitEntryRelation
{
    public required Guid EntryId { get; init; }
    
    public required Guid TraitId { get; init; }
}
