using System.Linq.Expressions;
using APP.DataModels.Trait;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class TraitService(LogContext context, IEmbedService embedService) : ServiceWithEmbeddingBase(context, embedService)
{
    public async Task CreateTrait(string description, Guid[]? relatedEntryIds)
    {
        var embedding = EmbedService.GenerateEmbedding(description);
        
        var trait = new Trait
        {
            Description = description,
            Embedding =  embedding,
        };
        
        LogContext.Traits.Add(trait);

        if(relatedEntryIds is not null)
        {
            foreach (var entryId in relatedEntryIds)
            {
                var relation = new TraitEntryRelation
                {
                    TraitId = trait.Id,
                    EntryId = entryId,
                };

                LogContext.TraitEntryRelations.Add(relation);
            }
        }
        
        await LogContext.SaveChangesAsync();
    }

    public async Task RemoveTrait(Guid traitId)
    {
        await LogContext.TraitEntryRelations
            .Where(r => r.TraitId == traitId)
            .ExecuteDeleteAsync();
    }

    public async Task<List<TraitModel>> GetRelatedTraits(Guid entryId)
    {
        return await LogContext.Traits
            .Where(t => LogContext.TraitEntryRelations
                .Any(rel => rel.EntryId == entryId && rel.TraitId == t.Id))
            .Select(TraitMaterializer)
            .ToListAsync();
    }

    public async Task RelateTraitToEntry(Guid traitId, Guid entryId)
    {
        var relation = new TraitEntryRelation
        {
            TraitId = traitId,
            EntryId = entryId,
        };
        
        LogContext.TraitEntryRelations.Add(relation);
        await LogContext.SaveChangesAsync();
    }
    
    private static readonly Expression<Func<Trait, TraitModel>> TraitMaterializer =
        t => new TraitModel(t.Id, t.Description, t.TimeStamp);
    
    public async Task<List<TraitModel>> GetSemanticTraits(string query, int outputLimit, float minScore)
    {
        return await GetSemantic(
            LogContext, 
            EmbedService, 
            TraitMaterializer,
            query,
            outputLimit,
            minScore);
    }
}
