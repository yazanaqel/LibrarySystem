using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.UserService;
public interface IUserService
{
	Task Register(UserViewModel user);
	Task<bool> ConfirmUserAccount(UserViewModel model);
	Task<User> Login(UserViewModel model);
	Task ResendEmail(UserViewModel model);
	Task<User> SelectUserByEmail(string email);
}
