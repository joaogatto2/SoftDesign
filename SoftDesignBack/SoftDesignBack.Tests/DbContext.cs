using Microsoft.EntityFrameworkCore;
using SoftDesignBack.Infrastructure.DbContext;

namespace SoftDesignBack.Tests
{
    public static class DbContext
    {
        public static AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
    }
}