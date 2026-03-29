using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace Domain.Entities;

public sealed class EntryChunk : IText, IId
{
    public Guid Id { get; init; }
    
    public Guid EntryId { get; init; }

    public required int Number { get; init; }
    
    public required string Text { get; init; }
    
    public int TextLength { get; set; }

    public required float[] Embedding { get; init; }

    public LogEntry Entry { get; init; } = null!;
}
