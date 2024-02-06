using Microsoft.AspNetCore.Identity;

namespace PersonDetection.Backend.Infrastructure.Models.Identity;

public sealed class User : IdentityUser
{
    public User(string userName, string email)
    {
        Id = Guid.NewGuid().ToString();
        
        UserName = userName;
        NormalizedUserName = userName.ToUpper();
        
        Email = email;
        NormalizedEmail = email.ToUpper();
    }
}