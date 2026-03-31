using System.Linq.Expressions;
using APP.DataModels;
using APP.DataModels.Entry;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class EntryService(LogContext context, IEmbedService embed) : ServiceWithEmbeddingBase(context, embed)
{
    private static readonly Expression<Func<EntryChunk, ChunkModel>> ChunkMaterializer =
        c => new ChunkModel(c.Text, c.EntryId, c.Entry.Summary, c.Entry.TimeStamp, c.Number, c.Entry.TotalChunks, c.Entry.TextLength)
        {
            Id = c.Id,
        };

    private static readonly Expression<Func<Entry, EntrySummaryModel>> EntrySummaryMaterializer =
        e => new EntrySummaryModel(e.Id, e.Summary, e.TextLength, e.TimeStamp);
    
    public async Task CreateEntry(string summary, string[] chunks, Guid[]? relatedTraitIds)
    {
        var entry = new Entry
        {
            Summary = summary,
            TotalChunks =  chunks.Length,
            Text = string.Join('\n', chunks)
        };

        for (int i = 0; i < chunks.Length; i++)
        {
            var embedding = EmbedService.GenerateEmbedding(chunks[i]);

            var chunk = new EntryChunk
            {
                Number = i + 1,
                Text = chunks[i],
                Embedding = embedding,
                Entry = entry
            };
            
            LogContext.EntryChunks.Add(chunk);
        }

        if (relatedTraitIds is not null)
        {
            foreach (var traitId in relatedTraitIds)
            {
                var relation = new TraitEntryRelation
                {
                    TraitId = traitId,
                    EntryId = entry.Id,
                };

                LogContext.TraitEntryRelations.Add(relation);
            }
        }
        
        LogContext.Add(entry);
        await LogContext.SaveChangesAsync();
    }

    public async Task RemoveEntry(Guid entryId)
    {
        await LogContext.Entries
            .Where(e=> e.Id == entryId)
            .ExecuteDeleteAsync();
    }
    
    public async Task<List<EntrySummaryModel>> GetRecentEntrySummaries(int outputLimit)
    {
        return await LogContext.Entries
            .AsNoTracking()
            .OrderByDescending(e => e.TimeStamp)
            .Take(outputLimit)
            .Select(EntrySummaryMaterializer)
            .ToListAsync();
    }

    public async Task<FullEntryModel?> GetFullEntry(Guid entryId)
    {
        return await LogContext.Entries
            .AsNoTracking()
            .Where(e => e.Id == entryId)
            .Select(e => new FullEntryModel(e.Id, e.Summary, e.Text, e.TimeStamp))
            .FirstOrDefaultAsync();
    }

    public async Task<List<EntrySummaryModel>> GetRelatedSummaries(Guid traitId)
    {
        return await LogContext.Entries
            .Where(e => LogContext.TraitEntryRelations
                .Any(rel => rel.EntryId == e.Id && rel.TraitId == traitId))
            .Select(EntrySummaryMaterializer)
            .ToListAsync();
    }
    
    public async Task<List<ChunkModel>> GetSemanticChunks(string query, int outputLimit, float minScore)
    {
        return await GetSemantic(
            LogContext,
            EmbedService,
            ChunkMaterializer,
            query,
            outputLimit,
            minScore);
    }

    public async Task<ChunkModel?> GetChunk(Guid entryId, int number)
    {
        return await LogContext.EntryChunks
            .AsNoTracking()
            .Where(c => c.EntryId == entryId && c.Number == number)
            .Select(ChunkMaterializer)
            .FirstOrDefaultAsync();
    }
}
