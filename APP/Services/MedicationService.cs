using System.Linq.Expressions;
using APP.DataModels.Medication;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class MedicationService(LogContext context, IEmbedService embed) : ServiceWithEmbeddingBase(context, embed)
{
    public async Task CreateMedication(string name, bool prescriptionRequired, string description, bool currentlyTaking)
    {
        var embedding = EmbedService.GenerateEmbedding(description);
        
        var medication = new Medication
        {
            Name = name,
            PrescriptionRequired = prescriptionRequired,
            CurrentlyTaking = currentlyTaking,
            Description = description,
            Embedding = embedding
        };
        
        LogContext.Medications.Add(medication);
        await LogContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateTakingStatus(Guid medicationId, bool newStatus)
    {
        var result = await LogContext.Medications
            .Where(med => med.Id == medicationId)
            .ExecuteUpdateAsync(prop =>
                prop.SetProperty(med => med.CurrentlyTaking, newStatus));

        return result != 0;
    }

    public async Task RemoveMedication(Guid medicationId)
    {
        await LogContext.Medications
            .Where(med => med.Id == medicationId)
            .ExecuteDeleteAsync();
    }

    private static readonly Expression<Func<Medication, MedicationModel>> MedicationMaterializer =
        med => new MedicationModel(med.Id, med.Name, med.Description, med.PrescriptionRequired, med.CurrentlyTaking);
    
    public async Task<List<MedicationModel>> GetSemanticMedication(string query, int outputLimit, float minScore)
    {
        return await GetSemantic(
            LogContext,
            EmbedService,
            MedicationMaterializer,
            query,
            outputLimit,
            minScore);
    }
}
