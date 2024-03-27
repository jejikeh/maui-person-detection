using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonDetection.Backend.Infrastructure.Models;

namespace PersonDetection.Backend.Infrastructure;

public class PersonDetectionDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<PhotoInBucket> Photos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}