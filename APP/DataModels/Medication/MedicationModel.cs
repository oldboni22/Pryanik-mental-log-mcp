namespace APP.DataModels.Medication;

public sealed record MedicationModel(
    Guid MedicationId, string Name, string Description, bool PrescriptionRequired, bool CurrentlyTaking);
