using Microsoft.AspNetCore.Identity;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;

namespace SoftDesignBack.Domain.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<IdentityResult> CreateAsync(CreateAccountDto model);
        Task<SignInResult> PasswordSignInAsync(CreateAccountDto model);
        Task<ApplicationUser?> FindByEmailAsync(string email);
    }
}