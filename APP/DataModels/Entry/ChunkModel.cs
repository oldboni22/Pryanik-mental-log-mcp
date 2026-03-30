using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace APP.DataModels.Entry;

public sealed record ChunkModel(
    string Text,
    Guid EntryId,
    string EntrySummary,
    DateTime TimeStamp,
    int Number,
    int TotalChunks,
    int EntryTextLength) : IId
{
    [JsonIgnore]
    public Guid Id { get; set; }
}
