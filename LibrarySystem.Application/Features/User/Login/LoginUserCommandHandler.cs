using LibrarySystem.Application.Features.User.Register;
using LibrarySystem.Domain.Repositories;
using MediatR;

namespace LibrarySystem.Application.Features.User.Login;

internal sealed class LoginUserCommandHandler(IUserService userService) : IRequestHandler<LoginUserCommand,Domain.Entities.User>
{
    private readonly IUserService _userService = userService;

    public async Task<Domain.Entities.User> Handle(LoginUserCommand command,CancellationToken cancellationToken)
    {

        var user = await _userService.Login(command.Request.Email.ToLower(),command.Request.Password);

        return user;

    }


}