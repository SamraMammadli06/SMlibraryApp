using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Presentation.Dtos;

namespace SMlibraryApp.Presentation.Controllers;
public class IdentityController : Controller
{
    private readonly IUserRepository userRepository;
    private readonly UserManager<IdentityUser> UserManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public IdentityController(IUserRepository userRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
        this.UserManager = userManager;
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
        var result = await signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Get", "Books");
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        var newUser = new IdentityUser
        {
            UserName = registerDto.UserName,
        };
        var result = await this.UserManager.CreateAsync(newUser, registerDto.Password);
        if (result is null)
        {
            return BadRequest(result);
        }
        System.Console.WriteLine(result);
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await this.signInManager.SignOutAsync();
        return base.RedirectToAction(actionName: "Index", controllerName: "Home");
    }
}