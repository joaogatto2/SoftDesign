using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Domain.Repositories;
using SoftDesignBack.Domain.Services;

namespace SoftDesignBack.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration configuration;
        private readonly IApplicationUserRepository applicationUserRepository;
        public AccountService(IApplicationUserRepository applicationUserRepository, IConfiguration configuration)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.configuration = configuration;
        }

        public async Task<IdentityResult> Create(CreateAccountDto model)
        {
            return await this.applicationUserRepository.CreateAsync(model);
        }

        public async Task<LoginResponseDto> Login(CreateAccountDto model)
        {
            var result = await applicationUserRepository.PasswordSignInAsync(model);

            if (result.Succeeded)
            {
                var user = await applicationUserRepository.FindByEmailAsync(model.Email);
                var token = GenerateJwtToken(user!);
                return new LoginResponseDto { Token = token, Success = true };
            }
            else
            {
                return new LoginResponseDto { Success = false, Error = "Invalid username or password" };
            }
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}