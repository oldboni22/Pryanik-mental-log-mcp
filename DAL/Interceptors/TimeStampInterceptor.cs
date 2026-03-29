using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Domain.Interceptors;

public sealed class TimeStampsInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetTimeStamps(eventData);
        
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        SetTimeStamps(eventData);
        
        return new ValueTask<InterceptionResult<int>>(result);
    }

    private static void SetTimeStamps(DbContextEventData eventData)
    {
        var entities = eventData!.Context!.ChangeTracker
            .Entries()
            .Where(entry => entry is
            {
                Entity: ITimeStamp,
                State: EntityState.Added
            })
            .ToList();

        foreach (var entry in entities)
        {
            var entity = (ITimeStamp)entry.Entity;
            
            entity.TimeStamp = DateTime.UtcNow;
        }
    }
}