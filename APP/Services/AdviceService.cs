using APP.DataModels;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class AdviceService(LogContext context, IEmbedService embed) : ServiceWithEmbeddingBase(embed)
{
    public async Task CreateAdvice(string topic, string summary, string text, Guid? sourceEntryId)
    {
        var embedding = EmbedService.GenerateEmbedding($"topic: {topic}, summary: {summary}");
        
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
    
    public async Task RemoveAdvice(Guid adviceId)
    {
        await context.Advices
            .Where(a => a.Id == adviceId)
            .ExecuteDeleteAsync();
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
            .Where(a => a.Id == adviceId)
            .Select(a => new FullAdviceModel(a.Id, a.Text, a.Topic, a.Summary, a.TimeStamp, a.SourceEntryId))
            .FirstOrDefaultAsync();
    }

    public async Task<List<AdviceSummaryModel>> GetSemanticAdviceSummary(string query, int outputLimit, float minScore)
    {
        var queryVec = EmbedService.GenerateEmbedding(query);
        
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
}