namespace SoftDesignBack.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SoftDesignBack.Domain.Entities;
    using SoftDesignBack.Domain.Repositories;

    using SoftDesignBack.Infrastructure.DbContext;
    using SoftDesignBack.Infrastructure.Repositories;


    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            });

            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            return services;
        }
    }
}