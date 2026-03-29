using Domain.Entities;
using Domain.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public sealed class LogContext : DbContext
{
    public static readonly string DbDir = Path.Combine(AppContext.BaseDirectory, "Data");
    
    private static readonly string DbPath = $"{DbDir}/log.db";
    
    public DbSet<LogEntry> Entries { get; set; }
    
    public DbSet<EntryChunk> EntryChunks { get; set; }
    
    public DbSet<Advice>  Advices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new TimeStampsInterceptor());

        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogContext).Assembly);
    }
}
