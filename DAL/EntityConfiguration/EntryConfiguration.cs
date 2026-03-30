using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfiguration;

public sealed class EntryConfiguration : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
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
        
        builder.HasMany(entry => entry.TraitRelations)
            .WithOne()
            .HasForeignKey(entry => entry.EntryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
