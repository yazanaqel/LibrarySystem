using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LibrarySystem.BLL.Services.UserService;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.ViewModels;
using LibrarySystem.BLL.Services.EmailService;

namespace LibrarySystem.PL.Controllers;
public class UserController : Controller
{
	private readonly IUserService _userService;
	private readonly IEmailService _emailService;

	public UserController(IUserService userService, IEmailService emailService)
	{
		_userService = userService;
		_emailService = emailService;
	}

	[HttpGet]
	public IActionResult ConfirmAccount() => View();

	[HttpPost]
	public async Task<IActionResult> ConfirmAccount(UserViewModel model)
	{
		try
		{
			bool result = await _userService.ConfirmUserAccount(model);

			if (!result)
			{
				ViewData["Error"] = "Incorrect token or email";
				return View();
			}

			User user = await _userService.SelectUserByEmail(model.Email);

			if (user is not null)
			{
				if (user.IsConfirmed)
				{
					ViewData["Confirmed"] = "Your account is confirmed";
					return View();
				}
			}

			return RedirectToAction(nameof(Login));
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}
	}
	public IActionResult Register() => View();

	[HttpPost]
	public async Task<IActionResult> Register(UserViewModel model)
	{
		try
		{
			User userCheck = await _userService.Login(model);

			if (userCheck is not null)
			{
				ViewData["Error"] = "Try Another Email!";
				return View();
			}

			await _userService.Register(model);
			return RedirectToAction(nameof(ConfirmAccount));
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}
	}

	public IActionResult ResendEmail() => View();

	[HttpPost]
	public async Task<IActionResult> ResendEmail(UserViewModel model)
	{
		try
		{
			User user = await _userService.SelectUserByEmail(model.Email);

			if (user is not null)
			{
				if (user.IsConfirmed)
				{
					ViewData["Error"] = "Your account is already confirmed";
					return View();
				}

				await _userService.ResendEmail(model);
				ViewData["SentEmail"] = "Please check your inbox";
				return RedirectToAction(nameof(ConfirmAccount));
			}


			ViewData["Error"] = "You are not registerd";
			return View();

		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}
	}

	public IActionResult Login()
	{
		ClaimsPrincipal claimUser = HttpContext.User;

		if (claimUser.Identity.IsAuthenticated)
			return RedirectToAction(nameof(Index), "Book");


		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(UserViewModel model)
	{
		try
		{
			User user = await _userService.Login(model);

			if (user is not null)
			{
				if (user.IsWrongPassword)
				{
					ViewData["Error"] = "Wrong Username or Password";
					return View();
				}

				if (!user.IsConfirmed)
				{
					ViewData["Error"] = "Confirm you account";
					return View();
				}

				await CreateClaim(user, model.KeepLoggedIn);

				return RedirectToAction(nameof(Index), "Book");
			}

			ViewData["Error"] = "Wrong Username or Password";
			return View();
		}
		catch (Exception)
		{
			ViewData["Error"] = "Something went wrong!";
			return View();
		}


	}


	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		return RedirectToAction(nameof(Login), "User");
	}


	private async Task CreateClaim(User user, bool rememberMe)
	{
		List<Claim> claims = new List<Claim>() {
					new Claim(ClaimTypes.NameIdentifier, user.Email),
					new Claim(ClaimTypes.Role, user.Role),
					new Claim(ClaimTypes.Name, user.Email)

			};

		ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
			CookieAuthenticationDefaults.AuthenticationScheme);

		AuthenticationProperties properties = new AuthenticationProperties()
		{

			AllowRefresh = true,
			IsPersistent = rememberMe
		};

		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
			new ClaimsPrincipal(claimsIdentity), properties);
	}
}
