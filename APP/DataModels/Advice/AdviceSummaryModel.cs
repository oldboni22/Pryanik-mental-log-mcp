using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace APP.DataModels.Advice;

public sealed record AdviceSummaryModel(
    Guid AdviceId,
    string Topic,
    string Summary,
    int TextLength,
    DateTime TimeStamp,
    Guid? EntryId) : IId
{
    [JsonIgnore]
    public Guid Id => AdviceId;
}
