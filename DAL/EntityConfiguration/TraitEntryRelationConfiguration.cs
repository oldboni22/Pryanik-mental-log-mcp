using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfiguration;

public sealed class TraitEntryRelationConfiguration : IEntityTypeConfiguration<TraitEntryRelation>
{
    public void Configure(EntityTypeBuilder<TraitEntryRelation> builder)
    {
        builder.HasKey(rel => new { rel.TraitId, rel.EntryId });
    }
}
