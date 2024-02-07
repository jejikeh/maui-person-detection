using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace PersonDetection.Backend.Application.Tests.Fakes;

public class FakeSignInManager : SignInManager<IdentityUser>
{            
    public FakeSignInManager() : base(
            new Mock<FakeUserManager>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
            new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
            new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<IdentityUser>>().Object
        )
    { }
}