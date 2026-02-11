using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LibrarySystem.Infrastructure.Repositories;

public class UserService(ApplicationDbContext applicationDbContext) : IUserService
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<bool> ConfirmUserAccount(User user)
    {
        user.IsConfirmed = true;

        _applicationDbContext.Users.Update(user);

        await _applicationDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User> Login(string email,string password)
    {

        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if(user == null)
        {
            return new User();
        }

        if(!VerifyPasswordHash(password,user.Password,user.PasswordSalt) || !user.IsConfirmed)
        {
            return new User
            {
                IsWrongPassword = user.IsWrongPassword,
                IsConfirmed = user.IsConfirmed
            };
        }

        return user;
    }

    public async Task Register(User user)
    {

        _applicationDbContext.Users.Add(user);

        await _applicationDbContext.SaveChangesAsync();
    }

    public Task ResendEmail(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if(user is null)
        {
            return new User();
        }

        return user;
    }

    private bool VerifyPasswordHash(string password,byte[] passwordHash,byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

}
