using APP.DataModels;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class EntryService(LogContext context, IEmbedService embed) : ServiceWithEmbeddingBase(embed)
{
    public async Task CreateEntry(string summary, string[] chunks)
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
            
            context.EntryChunks.Add(chunk);
        }
        
        context.Add(entry);
        await context.SaveChangesAsync();
    }

    public async Task RemoveEntry(Guid entryId)
    {
        await context.Entries
            .Where(e=> e.Id == entryId)
            .ExecuteDeleteAsync();
    }
    
    public async Task<List<EntrySummaryModel>> GetRecentEntrySummaries(int outputLimit)
    {
        return await context.Entries
            .AsNoTracking()
            .OrderByDescending(e => e.TimeStamp)
            .Take(outputLimit)
            .Select(e => new EntrySummaryModel(e.Id, e.Summary, e.TextLength, e.TimeStamp))
            .ToListAsync();
    }

    public async Task<FullEntryModel?> GetFullEntry(Guid entryId)
    {
        return await context.Entries
            .AsNoTracking()
            .Where(e => e.Id == entryId)
            .Select(e => new FullEntryModel(e.Id, e.Summary, e.Text, e.TimeStamp))
            .FirstOrDefaultAsync();
    }

    public async Task<List<ChunkModel>> GetSemanticChunks(string query, int outputLimit, float minScore)
    {
        var queryVec = EmbedService.GenerateEmbedding(query);
        
        var chunksMetadata = await context.EntryChunks
            .AsNoTracking()
            .Select(c => new
            {
                vec = c.Embedding,
                id =  c.Id,
            })
            .ToListAsync();
        
        var matchesIds = chunksMetadata
            .Select(res => new {id = res.id, score = CalculateSimilarity(queryVec, res.vec)})
            .Where(res => res.score >= minScore)
            .OrderByDescending(res => res.score)
            .Select(res => res.id)
            .Take(outputLimit)
            .ToList();
        
        var results = await context.EntryChunks
            .AsNoTracking()
            .Include(c => c.Entry)
            .Where(c => matchesIds.Contains(c.Id))
            .Select(c => new
            {
                model = new ChunkModel(
                    c.Text, c.EntryId, c.Entry.Summary, c.Entry.TimeStamp, c.Number, c.Entry.TotalChunks, c.Entry.TextLength),
                id = c.Id,
            })
            .ToListAsync();
        
        return results.OrderBy(r => matchesIds.IndexOf(r.id)).Select(r => r.model).ToList();
    }

    public async Task<ChunkModel?> GetChunk(Guid entryId, int number)
    {
        return await context.EntryChunks
            .AsNoTracking()
            .Where(c => c.EntryId == entryId && c.Number == number)
            .Select(c => new ChunkModel(
                c.Text, c.EntryId, c.Entry.Summary, c.Entry.TimeStamp,c.Number, c.Entry.TotalChunks, c.Entry.TextLength))
            .FirstOrDefaultAsync();
    }
}
