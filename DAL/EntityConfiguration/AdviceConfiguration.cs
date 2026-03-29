using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfiguration;

public sealed class AdviceConfiguration : IEntityTypeConfiguration<Advice>
{
    public void Configure(EntityTypeBuilder<Advice> builder)
    {
        builder.HasKey(advice => advice.Id);
        builder.FixGuidConversion();
    }
}
