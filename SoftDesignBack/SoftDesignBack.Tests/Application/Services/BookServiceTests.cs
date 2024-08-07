using Moq;
using SoftDesignBack.Application.Services;
using SoftDesignBack.Domain.Dtos;
using SoftDesignBack.Domain.Entities;
using SoftDesignBack.Domain.Repositories;

namespace SoftDesignBack.Tests.Application.Services
{
    public class BookServiceTests
    {
        [Fact]
        public async Task Create_Successfully()
        {
            var mockBookRepository = new Mock<IBookRepository>();
            var bookService = new BookService(mockBookRepository.Object);
            var createBookDto = new CreateBookDto { Name = "Test Book", Author = "Test Author" };
            var book = new Book { Id = 1, Name = "Test Book", Author = "Test Author" };

            mockBookRepository.Setup(repo => repo.Create(It.IsAny<CreateBookDto>())).ReturnsAsync(book);

            var result = await bookService.Create(createBookDto);

            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Name, result.Name);
            Assert.Equal(book.Author, result.Author);
        }

        [Fact]
        public async Task Get_Successfully()
        {
            var mockBookRepository = new Mock<IBookRepository>();
            var bookService = new BookService(mockBookRepository.Object);
            var book = new Book { Id = 1, Name = "Test Book", Author = "Test Author", TenantId = "uuuid-123", Tenant = new ApplicationUser { Id = "uuuid-123", Email = "tst@tst.com" } };

            mockBookRepository.Setup(repo => repo.Get(1)).ReturnsAsync(book);

            var result = await bookService.Get(1);

            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Name, result.Name);
            Assert.Equal(book.Author, result.Author);
            Assert.NotNull(result.Tenant);
            Assert.Equal(book.Tenant.Id, result.Tenant.Id);
            Assert.Equal(book.Tenant.Email, result.Tenant.Email);
        }

        [Fact]
        public async Task GetAll_Successfully()
        {
            var mockBookRepository = new Mock<IBookRepository>();
            var bookService = new BookService(mockBookRepository.Object);
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Book 1", Author = "Author 1", TenantId = "User1",  Tenant = new ApplicationUser { Id = "User1", Email = "tst@tst.com" } },
                new Book { Id = 2, Name = "Book 2", Author = "Author 2", TenantId = null },
                new Book { Id = 3, Name = "Book 3", Author = "Author 3", TenantId = "User2", Tenant = new ApplicationUser { Id = "User2", Email = "tst@tst.com" } }
            };

            mockBookRepository.Setup(repo => repo.GetAll()).ReturnsAsync(books);

            var result = await bookService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());

            var book1 = result.FirstOrDefault(x => x.Id == 1);
            Assert.NotNull(book1);
            Assert.Equal("Book 1", book1.Name);
            Assert.Equal("Author 1", book1.Author);
            Assert.NotNull(book1.Tenant);
            Assert.Equal("User1", book1.Tenant.Id);

            var book2 = result.FirstOrDefault(x => x.Id == 2);
            Assert.NotNull(book2);
            Assert.Equal("Book 2", book2.Name);
            Assert.Equal("Author 2", book2.Author);
            Assert.Null(book2.Tenant);

            var book3 = result.FirstOrDefault(x => x.Id == 3);
            Assert.NotNull(book3);
            Assert.Equal("Book 3", book3.Name);
            Assert.Equal("Author 3", book3.Author);
            Assert.NotNull(book3.Tenant);
            Assert.Equal("User2", book3.Tenant.Id);
        }

        [Fact]
        public async Task Rent_Successfully()
        {
            var mockBookRepository = new Mock<IBookRepository>();
            var bookService = new BookService(mockBookRepository.Object);
            var bookId = 1;
            var userId = "user123";
            var book = new Book { Id = bookId, Name = "Test Book", Author = "Test Author" };

            mockBookRepository.Setup(repo => repo.Get(bookId)).ReturnsAsync(book);
            mockBookRepository
                .Setup(repo => repo.Update(It.IsAny<Book>()))
                .Callback((Book b) => { b.TenantId = userId; })
                .Returns(Task.CompletedTask);

            await bookService.Rent(bookId, userId);

            Assert.Equal(userId, book.TenantId);
        }

        [Fact]
        public async Task Rent_Failed_Already_Rented()
        {
            var bookRepositoryMock = new Mock<IBookRepository>();
            var book = new Book { Id = 1, Name = "Test Book", Author = "Test Author", TenantId = "user456" };
            bookRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(book);
            var service = new BookService(bookRepositoryMock.Object);
            var userId = "user123";

            await Assert.ThrowsAsync<Exception>(async () => await service.Rent(1, userId));
        }

        
        [Fact]
        public async Task GiveBack_Successfully()
        {
            var mockBookRepository = new Mock<IBookRepository>();
            var bookService = new BookService(mockBookRepository.Object);
            var bookId = 1;
            var userId = "user123";
            var book = new Book { Id = bookId, Name = "Test Book", Author = "Test Author", TenantId = userId };

            mockBookRepository.Setup(repo => repo.Get(bookId)).ReturnsAsync(book);
            mockBookRepository
                .Setup(repo => repo.Update(It.IsAny<Book>()))
                .Callback((Book b) => { b.TenantId = null; })
                .Returns(Task.CompletedTask);

            await bookService.GiveBack(bookId, userId);

            Assert.Null(book.TenantId);
        }

        [Fact]
        public async Task GiveBack_Failed_Not_Rented()
        {
            var bookRepositoryMock = new Mock<IBookRepository>();
            var book = new Book { Id = 1, Name = "Test Book", Author = "Test Author", TenantId = null };
            bookRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(book);
            var service = new BookService(bookRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(() => service.GiveBack(1, "user123"));
        }

        [Fact]
        public async Task GiveBack_Failed_Rented_By_Other_User()
        {
            var bookRepositoryMock = new Mock<IBookRepository>();
            var book = new Book { Id = 1, Name = "Test Book", Author = "Test Author", TenantId = "user123" };
            bookRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(book);
            var service = new BookService(bookRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await service.GiveBack(1, "user456"));
        }
    }
}