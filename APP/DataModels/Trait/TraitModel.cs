using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace APP.DataModels.Trait;

public sealed record TraitModel(Guid TraitId, string Description, DateTime Timestamp) : IId
{
    [JsonIgnore]
    public Guid Id => TraitId;
}
