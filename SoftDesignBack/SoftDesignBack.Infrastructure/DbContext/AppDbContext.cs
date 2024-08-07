namespace SoftDesignBack.Infrastructure.DbContext
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using SoftDesignBack.Domain.Entities;

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Books)
                .WithOne(b => b.Tenant)
                .HasForeignKey(b => b.TenantId)
                .IsRequired(false);
        }
    }
}