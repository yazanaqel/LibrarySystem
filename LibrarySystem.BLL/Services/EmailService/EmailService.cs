using LibrarySystem.DAL.ViewModels;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace LibrarySystem.BLL.Services.EmailService;
public class EmailService: IEmailService
{
	private readonly IConfiguration configuration;

	public EmailService(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

	public async Task SendEmail(EmailViewModel request)
	{
		var email = new MimeMessage();
		email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailUsername").Value));
		email.To.Add(MailboxAddress.Parse(request.To));
		email.Subject = request.Subject;
		email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

		using var smptClient = new SmtpClient();
		smptClient.Connect(configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
		smptClient.Authenticate(configuration.GetSection("EmailUsername").Value, configuration.GetSection("EmailPassword").Value);
		await smptClient.SendAsync(email);
		smptClient.Disconnect(true);
	}

}
