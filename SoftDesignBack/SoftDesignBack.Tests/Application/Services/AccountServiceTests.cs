using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using SoftDesignBack.Application.Services;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Domain.Repositories;
using SoftDesignBack.Infrastructure.Repositories;

namespace SoftDesignBack.Tests.Application.Services
{
    public class AccountServiceTests
    {
        public AccountServiceTests()
        {
        }
        
        [Fact]
        public async Task Create_Successfully()
        {
            var mockRepo = new Mock<IApplicationUserRepository>();
            var accountService = new AccountService(mockRepo.Object, null);
            var createAccountDto = new CreateAccountDto { Email = "tst@tst.com", Password = "teste123" };
            
            mockRepo.Setup(repo => repo.CreateAsync(createAccountDto)).ReturnsAsync(IdentityResult.Success);

            var result = await accountService.Create(createAccountDto);

            Assert.Equal(IdentityResult.Success, result);
        }

        [Fact]
        public async Task Login_Successfully()
        {
            var mockRepo = new Mock<IApplicationUserRepository>();
            var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            var accountService = new AccountService(mockRepo.Object, mockConfig.Object);
            var createAccountDto = new CreateAccountDto { Email = "tst@tst.com", Password = "teste123" };
            var user = new ApplicationUser { Id = "1", Email = "tst@tst.com" };
            
            mockRepo.Setup(repo => repo.PasswordSignInAsync(createAccountDto)).ReturnsAsync(SignInResult.Success);
            mockRepo.Setup(repo => repo.FindByEmailAsync(createAccountDto.Email)).ReturnsAsync(user);
            mockConfig.Setup(config => config["Jwt:Key"]).Returns("super_secret_keysuper_secret_keysuper_secret_key");

            var result = await accountService.Login(createAccountDto);

            Assert.True(result.Success);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Login_Failed()
        {
            var mockRepo = new Mock<IApplicationUserRepository>();
            var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            var accountService = new AccountService(mockRepo.Object, mockConfig.Object);
            var loginDto = new CreateAccountDto { Email = "tst@tst.com", Password = "teste123" };
            
            mockRepo.Setup(repo => repo.PasswordSignInAsync(loginDto)).ReturnsAsync(SignInResult.Failed);
            
            var result = await accountService.Login(loginDto);
            
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Error);
        }
    }
}