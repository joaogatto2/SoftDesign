using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using SoftDesignBack.Domain.Entities;

namespace SoftDesignBack.Tests
{
    public class FakeSignInManager : SignInManager<ApplicationUser>
    {            
        public FakeSignInManager(Mock<UserManager<ApplicationUser>> userManagerMock)
            : base(
                userManagerMock.Object,
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object
                )
        { }
    }
}