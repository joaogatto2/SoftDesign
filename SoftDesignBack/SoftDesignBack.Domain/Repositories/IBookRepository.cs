using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;

namespace SoftDesignBack.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book> Create(CreateBookDto model);
        Task<Book?> Get(int id);
        Task<IEnumerable<Book>> GetAll();
        Task Update(Book book);
    }
}