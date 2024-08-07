namespace SoftDesignBack.Application
{
    using Microsoft.Extensions.DependencyInjection;
    using SoftDesignBack.Application.Services;
    using SoftDesignBack.Domain.Services;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBookService, BookService>();

            return services;
        }
    }
}