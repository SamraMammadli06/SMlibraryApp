using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Presentation.Dtos;

namespace SMlibraryApp.Presentation.Controllers;
public class IdentityController : Controller
{
    private readonly IUserRepository userRepository;

    public IdentityController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        base.ViewData["returnUrl"] = returnUrl;

        return base.View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register()
    {
        return base.View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        var user = await userRepository.FindUser(new User()
        {
            UserName = loginDto.UserName,
            Password = loginDto.Password,
        });
        if (user is null)
            return base.BadRequest("Incorrect user information");
        var claims = new Claim[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await base.HttpContext.SignInAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
            principal: new ClaimsPrincipal(claimsIdentity)
        );

        if (string.IsNullOrWhiteSpace(loginDto.ReturnUrl))
            return base.RedirectToAction(controllerName: "Books", actionName: "Get");

        return base.RedirectPermanent(loginDto.ReturnUrl);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        var count = await userRepository.Create(new User()
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = registerDto.Password,
        });
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}