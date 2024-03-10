using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersonDetection.Backend.Infrastructure.Models.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasMany(user => user.Photos)
            .WithOne(photo => photo.Owner)
            .HasForeignKey(photo => photo.OwnerId)
            .IsRequired();
    }
}