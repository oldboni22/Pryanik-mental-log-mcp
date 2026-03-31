using Domain.Entities;
using Domain.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public sealed class LogContext : DbContext
{
    public static readonly string DbDir = Path.Combine(AppContext.BaseDirectory, "Data");
    
    private static readonly string DbPath = Path.Combine(DbDir, "log.db");
    
    public DbSet<Entry> Entries { get; set; }
    
    public DbSet<EntryChunk> EntryChunks { get; set; }
    
    public DbSet<Advice>  Advices { get; set; }
    
    public DbSet<Trait>  Traits { get; set; }
    
    public DbSet<TraitEntryRelation>  TraitEntryRelations { get; set; }
    
    public DbSet<Medication>  Medications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new TimeStampsInterceptor(), new TextLengthInterceptor());

        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogContext).Assembly);
    }
}
