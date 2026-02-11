using LibrarySystem.Application.Features.User.ConfirmUserAccount;
using LibrarySystem.Application.Features.User.Login;
using LibrarySystem.Application.Features.User.Register;
using LibrarySystem.Application.Features.User.ResendEmail;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrarySystem.PL.Controllers;

public class UserController(IMediator mediator) : Controller
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public IActionResult ConfirmAccount() => View();

    [HttpPost]
    public async Task<IActionResult> ConfirmAccount(Models.User model)
    {
        try
        {
            var request = new ConfirmUserAccountRequest(model.Email,model.Token);

            bool result = await _mediator.Send(new ConfirmUserAccountCommand(request));

            if(!result)
            {
                ViewData["Error"] = "Incorrect token or email";

                return View();
            }

            ViewData["Confirmed"] = "Your account is confirmed";

            return RedirectToAction(nameof(Login));
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
    }
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(Models.User model)
    {
        try
        {
            var request = new RegisterUserRequest(model.Email,model.Password,model.ConfirmPassword);

            await _mediator.Send(new RegisterUserCommand(request));

            //if(userCheck is not null)
            //{
            //    ViewData["Error"] = "Try Another Email!";
            //    return View();
            //}


            return RedirectToAction(nameof(ConfirmAccount));
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
    }

    public IActionResult ResendEmail() => View();

    [HttpPost]
    public async Task<IActionResult> ResendEmail(Models.User model)
    {
        try
        {
            var request = new ResendEmailRequest(model.Email);

            await _mediator.Send(new ResendEmailCommand(request));

            //if(user is not null)
            //{
            //    if(user.IsConfirmed)
            //    {
            //        ViewData["Error"] = "Your account is already confirmed";
            //        return View();
            //    }

            //    await _userService.ResendEmail(model);
            //    ViewData["SentEmail"] = "Please check your inbox";
            //    return RedirectToAction(nameof(ConfirmAccount));
            //}


            ViewData["Error"] = "You are not registerd";
            return View();

        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }
    }

    public IActionResult Login()
    {
        ClaimsPrincipal claimUser = HttpContext.User;

        if(claimUser.Identity.IsAuthenticated)
            return RedirectToAction(nameof(Index),"Book");


        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Models.User model)
    {
        try
        {
            var request = new LoginUserRequest(model.Email,model.Password,model.KeepLoggedIn);

            var user = await _mediator.Send(new LoginUserCommand(request));

            if(user is not null)
            {
                if(user.IsWrongPassword)
                {
                    ViewData["Error"] = "Wrong Username or Password";
                    return View();
                }

                if(!user.IsConfirmed)
                {
                    ViewData["Error"] = "Confirm you account";
                    return View();
                }

                await CreateClaim(user,model.KeepLoggedIn);

                return RedirectToAction(nameof(Index),"Book");
            }

            ViewData["Error"] = "Wrong Username or Password";
            return View();
        }
        catch(Exception)
        {
            ViewData["Error"] = "Something went wrong!";
            return View();
        }


    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login),"User");
    }


    private async Task CreateClaim(Domain.Entities.User user,bool rememberMe)
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
            new ClaimsPrincipal(claimsIdentity),properties);
    }
}
