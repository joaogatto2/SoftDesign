using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;

namespace SoftDesignBack.Domain.Services
{
    public interface IBookService
    {
        Task<Book> Create(CreateBookDto model);
        Task<GetBookDto?> Get(int id);
        Task<IEnumerable<GetBookDto>> GetAll();
        Task Rent(int id, string userId);
        Task GiveBack(int id, string userId);
    }
}