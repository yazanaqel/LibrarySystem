using LibrarySystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Domain.Repositories;

public interface IUserService
{
    Task Register(User user);
    Task<bool> ConfirmUserAccount(User user);
    Task<User> Login(string email, string password);
    Task ResendEmail(User user);
    Task<User> GetUserByEmail(string email);
}
