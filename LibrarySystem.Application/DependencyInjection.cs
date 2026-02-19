using FluentValidation;
using LibrarySystem.Application.Behaviors;
using LibrarySystem.Application.MailService;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,IConfiguration configuration)
    {

        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));

        var emailSettings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>();

        services.AddFluentEmail(emailSettings.SenderEmail,emailSettings.SenderName)
            .AddSmtpSender(emailSettings.Host,emailSettings.Port,emailSettings.Username,emailSettings.Password);

        services.AddScoped<EmailService>();

        services.AddMediatR(options => options.RegisterServicesFromAssemblies(
            AssemblyProvider.GetAssembly()));

        services.AddValidatorsFromAssembly(AssemblyProvider.GetAssembly(),includeInternalTypes: true);

        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(LoggingBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

        //services.AddTransient(typeof(IPipelineBehavior<,>),typeof(CachingBehavior<,>));



        return services;
    }
}
