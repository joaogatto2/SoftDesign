using Microsoft.EntityFrameworkCore;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Domain.Repositories;
using SoftDesignBack.Domain.Services;

namespace SoftDesignBack.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        public async Task<Book> Create(CreateBookDto model)
        {
            return await bookRepository.Create(model);
        }

        public async Task<GetBookDto?> Get(int id)
        {
            var book = await bookRepository.Get(id);

            if (book == null)
            {
                return null;
            }
            return new GetBookDto { Id = book.Id, Name = book.Name, Author = book.Author, Tenant = book.TenantId == null ? null : new GetBookTenantDto { Id = book.TenantId, Email = book.Tenant.Email } };
        }

        public async Task<IEnumerable<GetBookDto>> GetAll()
        {
            return (await bookRepository.GetAll()).Select(x => new GetBookDto { Id = x.Id, Name = x.Name, Author = x.Author, Tenant = x.TenantId == null ? null : new GetBookTenantDto { Id = x.TenantId, Email = x.Tenant.Email } });
        }

        public async Task GiveBack(int id, string userId)
        {
            var book = await bookRepository.Get(id);

            if (book == null)
            {
                throw new Exception("Book not found");
            }

            if (book.TenantId == null)
            {
                throw new Exception("Book not rented");
            }

            if (book.TenantId != userId)
            {
                throw new Exception("Book not rented by user");
            }
            
            book.TenantId = null;
            await bookRepository.Update(book);
        }

        public async Task Rent(int id, string userId)
        {
            var book = await bookRepository.Get(id);

            if (book == null)
            {
                throw new Exception("Book not found");
            }

            if (book.TenantId != null)
            {
                throw new Exception("Book already rented");
            }
            
            book.TenantId = userId;

            await bookRepository.Update(book);
        }

    }
}