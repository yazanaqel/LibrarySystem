using LibrarySystem.Domain.Email;
using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.User.ConfirmUserAccount;

internal sealed class ConfirmUserAccountCommandHandler(IUserService userService,IEmailService emailService) : IRequestHandler<ConfirmUserAccountCommand,bool>
{
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;

    public async Task<bool> Handle(ConfirmUserAccountCommand command,CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmail(command.Request.Email);

        if(user is null || user.IsConfirmed)
        {
            return false;
        }

        var result = await _userService.ConfirmUserAccount(user);

        if(result)
        {
            Email request = new Email
            {
                To = command.Request.Email,
                Subject = "Thanks For Confirming Your Account :)",
                Body = $"<h1>{user.Token}<h1/>"
            };

            await _emailService.SendEmail(request);
        }

        return result;
    }

}
