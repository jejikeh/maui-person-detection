using Microsoft.AspNetCore.Identity;

namespace PersonDetection.Backend.Infrastructure.Models;

public class User : IdentityUser
{
    public ICollection<PhotoInBucket> Photos { get; set; }
}