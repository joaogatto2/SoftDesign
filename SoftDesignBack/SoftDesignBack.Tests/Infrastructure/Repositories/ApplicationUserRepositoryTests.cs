using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Infrastructure.Repositories;

namespace SoftDesignBack.Tests.Infrastructure.Repositories
{
    public class ApplicationUserRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_Successfully()
        {
            var userManagerMock = UserManager.MockUserManager<ApplicationUser>();
            var repository = new ApplicationUserRepository(userManagerMock.Object, null);
            var model = new CreateAccountDto { Email = "tst@tst.com", Password = "teste123" };
            var identityResult = IdentityResult.Success;

            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(identityResult);

            var result = await repository.CreateAsync(model);
            Assert.Equal(identityResult, result);
            userManagerMock.Verify(um => um.CreateAsync(It.Is<ApplicationUser>(u => u.Email == model.Email), model.Password), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_FailedExistingEmail()
        {
            var userManagerMock = UserManager.MockUserManager<ApplicationUser>();
            var repository = new ApplicationUserRepository(userManagerMock.Object, null);
            var model = new CreateAccountDto { Email = "tst@tst.com", Password = "teste123" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Email already exists" });

            userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(identityResult);

            var result = await repository.CreateAsync(model);

            Assert.Equal(identityResult, result);
            userManagerMock.Verify(um => um.CreateAsync(It.Is<ApplicationUser>(u => u.Email == model.Email), model.Password), Times.Once);
        }

        [Fact]
        public async Task FindByEmailAsync_Successfully()
        {
            var userManagerMock = UserManager.MockUserManager<ApplicationUser>();
            var repository = new ApplicationUserRepository(userManagerMock.Object, null);
            var email = "tst@tst.com";
            var user = new ApplicationUser { UserName = email, Email = email };

            userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);

            var result = await repository.FindByEmailAsync(email);

            Assert.Equal(user, result);
            userManagerMock.Verify(um => um.FindByEmailAsync(email), Times.Once);
        }
        
        [Fact]
        public async Task PasswordSignInAsync_Successfully()
        {
            var userManagerMock = UserManager.MockUserManager<ApplicationUser>();
            var signInManagerMock = new Mock<FakeSignInManager>(userManagerMock);
            var repository = new ApplicationUserRepository(userManagerMock.Object, signInManagerMock.Object);
            var model = new CreateAccountDto { Email = "tst@tst.com", Password = "teste123" };
            var signInResult = SignInResult.Success;

            signInManagerMock.Setup(sm => sm.PasswordSignInAsync(model.Email, model.Password, false, false)).ReturnsAsync(signInResult);

            var result = await repository.PasswordSignInAsync(model);

            Assert.Equal(signInResult, result);
            signInManagerMock.Verify(sm => sm.PasswordSignInAsync(model.Email, model.Password, false, false), Times.Once);
        }
    }

}