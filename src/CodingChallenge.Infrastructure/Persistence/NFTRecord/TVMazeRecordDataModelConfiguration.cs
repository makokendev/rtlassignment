using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodingChallenge.Infrastructure.Persistence.TVMazeRecord;
public class TVMazeRecordDataModelConfiguration : IEntityTypeConfiguration<TVMazeRecordDataModel>
{
    public void Configure(EntityTypeBuilder<TVMazeRecordDataModel> builder)
    {
        builder.HasKey(m => m.TVMazeIndex);
        builder.Property(t => t.TVMazeIndex)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.TVMazeType)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.Created)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(200);
        builder.Property(t => t.LastModified)
            .HasMaxLength(200);
    }
}