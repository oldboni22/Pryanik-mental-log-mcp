using APP.DataModels;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP;

public sealed class LogService(LogContext context, IEmbedService embed)
{
    public async Task CreateEntry(string summary, string[] chunks)
    {
        var entry = new LogEntry
        {
            Summary = summary,
            Text = string.Join('\n', chunks)
        };

        for (int i = 0; i < chunks.Length; i++)
        {
            var embedding = embed.GenerateEmbedding(chunks[i]);

            var chunk = new EntryChunk
            {
                Number = i + 1,
                Text = chunks[i],
                Embedding = embedding,
                TotalChunks = chunks.Length,
                Entry = entry
            };
            
            context.EntryChunks.Add(chunk);
        }
        
        context.Add(entry);
        await context.SaveChangesAsync();
    }
    
    public async Task CreateAdvice(string topic, string summary, string text, Guid? sourceEntryId)
    {
        var embedding = embed.GenerateEmbedding($"topic: {topic}, summary: {summary}");
        
        var advice = new Advice
        {
            Topic = topic,
            Summary = summary,
            Text = text,
            SourceEntryId = sourceEntryId,
            Embedding = embedding
        };
        
        context.Advices.Add(advice);
        await context.SaveChangesAsync();
    }

    public async Task RemoveEntry(Guid entryId)
    {
        await context.Entries
            .Where(e=> e.Id == entryId)
            .ExecuteDeleteAsync();
    }

    public async Task RemoveAdvice(Guid adviceId)
    {
        await context.Advices
            .Where(a => a.Id == adviceId)
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
            .Select(e => new FullEntryModel(e.Id, e.Summary, e.Text, e.TimeStamp))
            .FirstOrDefaultAsync(e => e.EntryId == entryId);
    }

    public async Task<List<ChunkModel>> GetSemanticChunks(string query, int outputLimit, float minScore)
    {
        var queryVec = embed.GenerateEmbedding(query);
        
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
                    c.Text, c.EntryId, c.Entry.Summary, c.Entry.TimeStamp, c.Number, c.TotalChunks),
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
                c.Text, c.EntryId, c.Entry.Summary, c.Entry.TimeStamp,c.Number, c.TotalChunks))
            .FirstOrDefaultAsync();
    }

    public async Task<List<AdviceSummaryModel>> GetRecentAdviceSummaries(int outputLimit)
    {
        return await context.Advices
            .AsNoTracking()
            .OrderByDescending(a => a.TimeStamp)
            .Take(outputLimit)
            .Select(a => new AdviceSummaryModel(a.Id, a.Topic, a.Summary, a.TextLength, a.TimeStamp, a.SourceEntryId))
            .ToListAsync();
    }

    public async Task<FullAdviceModel?> GetFullAdvice(Guid  adviceId)
    {
        return await context.Advices
            .AsNoTracking()
            .Select(a => new FullAdviceModel(a.Id, a.Text, a.Topic, a.Summary, a.TimeStamp, a.SourceEntryId))
            .FirstOrDefaultAsync(a => a.AdviceId == adviceId);
    }

    public async Task<List<AdviceSummaryModel>> GetSemanticAdviceSummary(string query, int outputLimit, float minScore)
    {
        var queryVec = embed.GenerateEmbedding(query);
        
        var advicesMetadata = await context.Advices
            .AsNoTracking()
            .Select(a => new
            {
                vec = a.Embedding,
                id = a.Id,
            })
            .ToListAsync();
        
        var matchesIds = advicesMetadata
            .Select(res => new {res.id, score = CalculateSimilarity(queryVec, res.vec)})
            .Where(res => res.score >= minScore)
            .OrderByDescending(res => res.score)
            .Select(res => res.id)
            .Take(outputLimit)
            .ToList();

        var results = await context.Advices
            .AsNoTracking()
            .Where(a => matchesIds.Contains(a.Id))
            .Select(a => new AdviceSummaryModel(a.Id, a.Topic, a.Summary, a.TextLength, a.TimeStamp, a.SourceEntryId))
            .ToListAsync();
        
        return results.OrderBy(r => matchesIds.IndexOf(r.AdviceId)).ToList();
    }

    public async Task<List<AdviceSummaryModel>> GetEntryAdviceSummaries(Guid entryId)
    {
        return await context.Advices
            .AsNoTracking()
            .Where(a => a.SourceEntryId == entryId)
            .Select(a => new AdviceSummaryModel(a.Id, a.Topic, a.Summary, a.TextLength, a.TimeStamp, a.SourceEntryId))
            .ToListAsync();
    }
    
    private static float CalculateSimilarity(float[] v1, float[] v2)
    {
        return v1.Zip(v2, (a, b) => a * b).Sum();
    }
}
