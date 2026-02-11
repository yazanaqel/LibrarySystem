using LibrarySystem.Domain.Repositories;
using LibrarySystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("SqlLiteConnectionString"));
        });

        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IBookService,BookService>();
        services.AddScoped<IBorrowingService,BorrowingService>();


        return services;
    }
}