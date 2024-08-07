using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Services;

namespace SoftDesignBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] CreateAccountDto model)
        {
            var response = await accountService.Login(model);
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto model)
        {
            var response = await accountService.Create(model);
            return Ok(response);
        }
        
        [HttpGet]
        [Route("tst")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult tst()
        {
            return Ok("tst");
        }
    }
}