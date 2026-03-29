using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityConfiguration;

public sealed class ChunkConfiguration : IEntityTypeConfiguration<EntryChunk>
{
    public void Configure(EntityTypeBuilder<EntryChunk> builder)
    {
        builder.HasKey(chunk => chunk.Id);
        
        builder.HasIndex(chunk => new{ chunk.EntryId, chunk.Number }).IsUnique();
    }
}
