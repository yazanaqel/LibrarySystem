using LibrarySystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Application.Features.User.GetUser;

internal class GetUserCommandHandler(IUserService userService) : IRequestHandler<GetUserCommand,GetUserResponse>
{
    private readonly IUserService _userService = userService;

    public async Task<GetUserResponse> Handle(GetUserCommand request,CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmail(request.Email);

        return new GetUserResponse
        (
            user.Id,
            user.Email,
             user.Role
        );
    }
}
