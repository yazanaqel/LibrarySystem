using FluentEmail.Core;
namespace LibrarySystem.Application.MailService;

public class EmailService(IFluentEmail email)
{

    private readonly IFluentEmail _email = email;
    public async Task SendEmailAsync(string to,string subject,string body)
    {
        await _email.To(to).Subject(subject).Body(body,isHtml: true).SendAsync();
    }

}

