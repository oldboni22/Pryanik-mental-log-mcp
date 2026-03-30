using System.Linq.Expressions;
using APP.DataModels;
using APP.DataModels.Advice;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public sealed class AdviceService(LogContext context, IEmbedService embed) : ServiceWithEmbeddingBase(context, embed)
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
        
        LogContext.Advices.Add(advice);
        await LogContext.SaveChangesAsync();
    }
    
    public async Task RemoveAdvice(Guid adviceId)
    {
        await LogContext.Advices
            .Where(a => a.Id == adviceId)
            .ExecuteDeleteAsync();
    }
    
    public async Task<List<AdviceSummaryModel>> GetRecentAdviceSummaries(int outputLimit)
    {
        return await LogContext.Advices
            .AsNoTracking()
            .OrderByDescending(a => a.TimeStamp)
            .Take(outputLimit)
            .Select(a => new AdviceSummaryModel(a.Id, a.Topic, a.Summary, a.TextLength, a.TimeStamp, a.SourceEntryId))
            .ToListAsync();
    }
    
    public async Task<FullAdviceModel?> GetFullAdvice(Guid  adviceId)
    {
        return await LogContext.Advices
            .AsNoTracking()
            .Where(a => a.Id == adviceId)
            .Select(a => new FullAdviceModel(a.Id, a.Text, a.Topic, a.Summary, a.TimeStamp, a.SourceEntryId))
            .FirstOrDefaultAsync();
    }

    private static readonly Expression<Func<Advice, AdviceSummaryModel>> AdviceSummaryMaterializer =
        a => new AdviceSummaryModel(a.Id, a.Topic, a.Summary, a.TextLength, a.TimeStamp, a.SourceEntryId);
    
    public async Task<List<AdviceSummaryModel>> GetSemanticAdviceSummary(string query, int outputLimit, float minScore)
    {
        return await GetSemantic(
            LogContext,
            EmbedService,
            AdviceSummaryMaterializer,
            query,
            outputLimit,
            minScore
        );
    }

    public async Task<List<AdviceSummaryModel>> GetEntryAdviceSummaries(Guid entryId)
    {
        return await LogContext.Advices
            .AsNoTracking()
            .Where(a => a.SourceEntryId == entryId)
            .Select(a => new AdviceSummaryModel(a.Id, a.Topic, a.Summary, a.TextLength, a.TimeStamp, a.SourceEntryId))
            .ToListAsync();
    }
}