using Microsoft.EntityFrameworkCore;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Infrastructure.Repositories;

namespace SoftDesignBack.Tests.Infrastructure.Repositories
{
    public class BookRepositoryTests
    {
        [Fact]
        public async Task Create_SuccessfullyCreatesBook()
        {
            var context = DbContext.GetContext();
            var repository = new BookRepository(context);
            var createBookDto = new CreateBookDto { Author = "Author Name", Name = "Book Name" };

            var result = await repository.Create(createBookDto);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal("Author Name", result.Author);
            Assert.Equal("Book Name", result.Name);
        }

        [Fact]
        public async Task Create_WithMissingFieldsThrowsException()
        {
            var context = DbContext.GetContext();
            var repository = new BookRepository(context);
            var model = new CreateBookDto { Author = null, Name = null };

            await Assert.ThrowsAsync<DbUpdateException>(() => repository.Create(model));
        }

        [Fact]
        public async Task Get_Successfully()
        {
            var context = DbContext.GetContext();
            var repository = new BookRepository(context);
            var book = new Book { Author = "Test Author", Name = "Test Book" };
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(book.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal("Test Author", result.Author);
            Assert.Equal("Test Book", result.Name);
        }

        [Fact]
        public async Task GetAll_Successfully()
        {
            var context = DbContext.GetContext();
            var repository = new BookRepository(context);
            var book1 = new Book { Author = "Author 1", Name = "Book 1" };
            var book2 = new Book { Author = "Author 2", Name = "Book 2" };
            await context.Books.AddRangeAsync(book1, book2);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Author == "Author 1" && b.Name == "Book 1");
            Assert.Contains(result, b => b.Author == "Author 2" && b.Name == "Book 2");
        }

        [Fact]
        public async Task Update_Successfully()
        {
            var context = DbContext.GetContext();
            var repository = new BookRepository(context);
            var book = new Book { Author = "Author Name", Name = "Book Name" };
            await repository.Create(new CreateBookDto { Author = book.Author, Name = book.Name });

            book.Name = "New Book Name";
            await repository.Update(book);
            var updatedBook = await repository.Get(book.Id);

            Assert.NotNull(updatedBook);
            Assert.Equal("Author Name", updatedBook.Author);
            Assert.Equal("New Book Name", updatedBook.Name);
        }
    }
}