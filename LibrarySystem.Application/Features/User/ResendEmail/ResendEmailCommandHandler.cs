using LibrarySystem.Application.MailService;
using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.User.ResendEmail;

internal sealed class ResendEmailCommandHandler(IUserService userService,EmailService emailService) : IRequestHandler<ResendEmailCommand>
{
    private readonly IUserService _userService = userService;
    private readonly EmailService _emailService = emailService;
    public async Task Handle(ResendEmailCommand command,CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmail(command.Request.Email);

        if(user is null)
        {
            return;
        }

        await _emailService.SendEmailAsync(command.Request.Email,"Confirm Your Email",$"<h1>{user.Token}<h1/>");

    }

}
