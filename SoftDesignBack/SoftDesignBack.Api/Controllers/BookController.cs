using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Services;

namespace SoftDesignBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;


        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] CreateBookDto model)
        {
            return Ok(await bookService.Create(model));
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await bookService.GetAll());
        }
        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        
        [HttpPut("{id}/rent")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Rent(int id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            await bookService.Rent(id, userId!);
            return NoContent();
        }
        
        [HttpPut("{id}/give-back")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GiveBack(int id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            await bookService.GiveBack(id, userId!);
            return NoContent();
        }
    }
}