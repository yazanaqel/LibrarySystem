using LibrarySystem.Application.MailService;
using LibrarySystem.Domain.Repositories;
using MediatR;
using System.Security.Cryptography;

namespace LibrarySystem.Application.Features.User.Register;

internal sealed class RegisterUserCommandHandler(IUserService userService,EmailService emailService) : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserService _userService = userService;
    private readonly EmailService _emailService = emailService;

    public async Task Handle(RegisterUserCommand command,CancellationToken cancellationToken)
    {

        var existingUser = await _userService.GetUserByEmail(command.Request.Email);

        if(existingUser.Email == command.Request.Email)
        {
            return;
        }

        CreatePasswordHash(command.Request.Password,out byte[] passwordHash,out byte[] passwordSalt);


        Random random = new Random();
        int randomNumber = random.Next(1000000);
        string token = randomNumber.ToString("D6");


        await _userService.Register(new Domain.Entities.User
        {

            Email = command.Request.Email.ToLower(),
            Password = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "User",
            Token = token

        });

        await _emailService.SendEmailAsync(command.Request.Email,"Confirm Your Email",$"<h1>{token}<h1/>");
    }
    private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

}
