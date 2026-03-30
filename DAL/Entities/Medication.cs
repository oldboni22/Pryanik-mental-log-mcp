using Domain.Interfaces;

namespace Domain.Entities;

public sealed class Medication : IId
{
    public Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public float[] Embedding { get; init; }
    
    public bool CurrentlyTaking  { get; set; }
    
    public bool PrescriptionRequired { get; init; }
}
