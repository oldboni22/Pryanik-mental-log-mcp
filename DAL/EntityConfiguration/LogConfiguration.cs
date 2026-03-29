using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfiguration;

public sealed class LogConfiguration : IEntityTypeConfiguration<LogEntry>
{
    public void Configure(EntityTypeBuilder<LogEntry> builder)
    {
        builder.HasKey(entry => entry.Id);
        builder.FixGuidConversion();
        
        builder
            .HasMany(entry => entry.Chunks)
            .WithOne(chunk => chunk.Entry)
            .HasForeignKey(chunk => chunk.EntryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(entry => entry.Advices)
            .WithOne(advice => advice.SourceEntry)
            .HasForeignKey(advice => advice.SourceEntryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
