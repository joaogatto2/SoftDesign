using Microsoft.AspNetCore.Identity;
using SoftDesignBack.Domain.Dtos;

namespace SoftDesignBack.Domain.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(CreateAccountDto model);
        Task<LoginResponseDto> Login(CreateAccountDto model);
    }
}