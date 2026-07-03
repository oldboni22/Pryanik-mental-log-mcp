using System.Buffers;
using System.Linq.Expressions;
using System.Numerics.Tensors;
using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APP.Services;

public abstract class ServiceWithEmbeddingBase(LogContext context, IEmbedService embedService)
{
    protected LogContext LogContext => context;
    
    protected IEmbedService EmbedService => embedService;
    
    protected async Task<List<TModel>> GetSemantic<TEntity, TModel>(
        Expression<Func<TEntity, TModel>> materializer,
        string query,
        int outputLimit, 
        float minScore)
        where TEntity : class, IId, IEmbedding
        where TModel : class, IId
    {
        var queryVec = embedService.GenerateEmbedding(query);

        var priorityQueue = new PriorityQueue<Guid, float>(Comparer<float>.Create((x, y) => y.CompareTo(x)));

        var queryStream = context.Set<TEntity>()
            .AsNoTracking()
            .Select(x => new { x.Id, x.Embedding })
            .AsAsyncEnumerable();

        await foreach (var row in queryStream)
        {
            var score = TensorPrimitives.Dot(queryVec, row.Embedding);

            if (score >= minScore)
            {
                priorityQueue.Enqueue(row.Id, score);
            }
        }

        var matchesIds = new List<Guid>(outputLimit);
        while (priorityQueue.TryDequeue(out var id, out _) && matchesIds.Count < outputLimit)
        {
            matchesIds.Add(id);
        }

        if (matchesIds.Count == 0) return [];

        var results = await context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => matchesIds.Contains(x.Id))
            .Select(materializer)
            .ToListAsync();

        var idOrderMap = results
            .Select((m, index) => (m.Id, index))
            .ToDictionary(x => x.Id, x => x.index);

        return results.OrderBy(r => idOrderMap[r.Id]).ToList();
    }
}
