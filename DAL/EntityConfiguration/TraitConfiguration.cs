using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfiguration;

public sealed class TraitConfiguration : IEntityTypeConfiguration<Trait>
{
    public void Configure(EntityTypeBuilder<Trait> builder)
    {
        builder.HasKey(trait => trait.Id);
        builder.FixGuidConversion();
        
        builder.HasMany(trait => trait.TraitRelations)
            .WithOne()
            .HasForeignKey(trait => trait.TraitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
