using LibrarySystem.Domain.Email;
using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.User.ResendEmail;

internal sealed class ResendEmailCommandHandler(IUserService userService,IEmailService emailService) : IRequestHandler<ResendEmailCommand>
{
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;
    public async Task Handle(ResendEmailCommand command,CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmail(command.Request.Email);

        if(user is null)
        {
            return;
        }

        Email request = new Email
        {
            To = command.Request.Email,
            Subject = "Confirm Your Email",
            Body = $"<h1>{user.Token}<h1/>"
        };

        await _emailService.SendEmail(request);
    }

}
