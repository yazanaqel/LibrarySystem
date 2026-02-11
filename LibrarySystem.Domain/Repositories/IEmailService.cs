namespace LibrarySystem.Domain.Repositories;

public interface IEmailService
{
    Task SendEmail(Email.Email request);
}
