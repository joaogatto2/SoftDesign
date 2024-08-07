using Microsoft.AspNetCore.Identity;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Domain.Repositories;

namespace SoftDesignBack.Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            
        }

        public async Task<IdentityResult> CreateAsync(CreateAccountDto model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            return await userManager.CreateAsync(user, model.Password);
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> PasswordSignInAsync(CreateAccountDto model)
        {
            return await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
        }
    }
}