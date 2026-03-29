using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Domain.Interceptors;

public sealed class TextLengthInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetLength(eventData);
        
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        SetLength(eventData);
        
        return new ValueTask<InterceptionResult<int>>(result);
    }

    private static void SetLength(DbContextEventData eventData)
    {
        var entities = eventData!.Context!.ChangeTracker
            .Entries()
            .Where(entry => entry is
            {
                Entity: IText,
                State: EntityState.Added
            })
            .ToList();

        foreach (var entry in entities)
        {
            var entity = (IText)entry.Entity;
            
            entity.TextLength = entity.Text.Length;
        }
    }
}
