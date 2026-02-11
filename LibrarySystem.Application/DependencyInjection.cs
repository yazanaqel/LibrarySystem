using FluentValidation;
using LibrarySystem.Application.MailService;
using LibrarySystem.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(options => options.RegisterServicesFromAssemblies(
            AssemblyProvider.GetAssembly()));

        services.AddValidatorsFromAssembly(AssemblyProvider.GetAssembly(),includeInternalTypes: true);

        services.AddScoped<IEmailService,EmailService>();

        return services;
    }
}
