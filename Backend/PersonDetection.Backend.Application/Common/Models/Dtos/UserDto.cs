using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Application.Common.Models.Requests.Login;

namespace PersonDetection.Backend.Application.Common.Models.Dtos;

public record UserDto(string UserName, string Email)
{
    public static UserDto FromIdentityUser(IdentityUser user) => 
        new UserDto(
            user.UserName ?? string.Empty, 
            user.Email ?? string.Empty);
}
