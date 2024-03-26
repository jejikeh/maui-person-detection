using Microsoft.AspNetCore.Identity;

namespace PersonDetection.Backend.Infrastructure.Common.Options;

public class IdentityModelOptions
{
    public UserOptions User { get; set; } = new UserOptions();
    public PasswordOptions Password { get; set; } = new PasswordOptions();
    public SignInOptions SignIn { get; set; } = new SignInOptions();
    public int MaxUserNameLength { get; set; } = 50;
}