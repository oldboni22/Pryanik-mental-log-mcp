using APP.DataModels.Medication;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class MedicationService(LogContext context, IEmbedService embed) : ServiceWithEmbeddingBase(embed)
{
    public async Task CreateMedication(string name, string prescriptionRequired, string description, bool currentlyTaking)
    {
        var embedding = EmbedService.GenerateEmbedding(description);
        
        var medication = new Medication
        {
            Name = name,
            PrescriptionRequired = currentlyTaking,
            CurrentlyTaking = currentlyTaking,
            Description = description,
            Embedding = embedding
        };
        
        context.Medications.Add(medication);
        await context.SaveChangesAsync();
    }

    public async Task<bool> UpdateTakingStatus(Guid medicationId, bool newStatus)
    {
        var result = await context.Medications
            .Where(med => med.Id == medicationId)
            .ExecuteUpdateAsync(prop =>
                prop.SetProperty(med => med.CurrentlyTaking, newStatus));

        return result != 0;
    }

    public async Task RemoveMedication(Guid medicationId)
    {
        await context.Medications
            .Where(med => med.Id == medicationId)
            .ExecuteDeleteAsync();
    }

    public async Task<List<MedicationModel>> GetSemanticMedication(string query, int outputLimit, float minScore)
    {
        var queryVec = EmbedService.GenerateEmbedding(query);
        
        var medsMetadata = await context.Medications
            .AsNoTracking()
            .Select(med => new { id = med.Id, vec = med.Embedding })
            .ToListAsync();

        var matchesIds = medsMetadata
            .Select(x => new { x.id, score = CalculateSimilarity(queryVec, x.vec) })
            .Where(x => x.score >= minScore)
            .OrderByDescending(x => x.score)
            .Select(x => x.id)
            .Take(outputLimit)
            .ToList();
        
        return await context.Medications
            .Where(med => matchesIds.Contains(med.Id))
            .Select(med => 
                new MedicationModel(med.Id, med.Name, med.Description, med.PrescriptionRequired, med.CurrentlyTaking))
            .ToListAsync();
    }
}
