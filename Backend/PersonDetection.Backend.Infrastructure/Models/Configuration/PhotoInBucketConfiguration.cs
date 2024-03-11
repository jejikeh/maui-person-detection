using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersonDetection.Backend.Infrastructure.Models.Configuration;

public class PhotoInBucketConfiguration : IEntityTypeConfiguration<PhotoInBucket>
{
    public void Configure(EntityTypeBuilder<PhotoInBucket> builder)
    {
        builder
            .Property(photo => photo.Id)
            .HasColumnType("int")
            .ValueGeneratedOnAdd();
    }
}