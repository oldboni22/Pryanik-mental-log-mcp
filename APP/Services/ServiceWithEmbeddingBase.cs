using System.Linq.Expressions;
using APP.DataModels.Advice;
using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public abstract class ServiceWithEmbeddingBase(LogContext context, IEmbedService embedService)
{
    protected LogContext LogContext => context;
    
    protected IEmbedService EmbedService => embedService;
    
    protected static float CalculateSimilarity(float[] v1, float[] v2)
    {
        return v1.Zip(v2, (a, b) => a * b).Sum();
    }

    protected static async Task<List<TModel>> GetSemantic<TEntity, TModel>(
        LogContext context,
        IEmbedService embedService,
        Expression<Func<TEntity, TModel>> materializer,
        string query, int outputLimit, float minScore)
        where TEntity : class, IId, IEmbedding
        where TModel : class, IId
    {
        var queryVec = embedService.GenerateEmbedding(query);

        var metadata = await context.Set<TEntity>()
            .AsNoTracking()
            .Select(x => new
            {
                vec = x.Embedding,
                id = x.Id,
            })
            .ToListAsync();

        var matchesIds = metadata
            .Select(res => new { res.id, score = CalculateSimilarity(queryVec, res.vec) })
            .Where(res => res.score >= minScore)
            .OrderByDescending(res => res.score)
            .Select(res => res.id)
            .Take(outputLimit)
            .ToList();

        var results = await context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => matchesIds.Contains(x.Id))
            .Select(materializer)
            .ToListAsync();

        return results.OrderBy(r => matchesIds.IndexOf(r.Id)).ToList();
    }
}
