using System.Text.Json.Serialization;
using Domain.Interfaces;

namespace APP.DataModels.Medication;

public sealed record MedicationModel(
    Guid MedicationId, string Name, string Description, bool PrescriptionRequired, bool CurrentlyTaking) : IId
{
    [JsonIgnore]
    public Guid Id => MedicationId;
}
