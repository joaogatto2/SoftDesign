using Microsoft.EntityFrameworkCore;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Domain.Repositories;
using SoftDesignBack.Infrastructure.DbContext;

namespace SoftDesignBack.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext context;


        public BookRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<Book> Create(CreateBookDto model)
        {
            var book = new Book
            {
                Author = model.Author,
                Name = model.Name
            };
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> Get(int id)
        {
            return await context.Books.Include(x => x.Tenant).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await context.Books.Include(x => x.Tenant).ToListAsync();
        }

        public async Task Update(Book book)
        {
            context.Books.Update(book);
            await context.SaveChangesAsync();
        }
    }
}