using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Presentation.Dtos;
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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromForm] PassChangeDto changeDto)
    {
        var user = await userRepository.FindUser(changeDto.Username);
        if(user is null){
            return BadRequest("Incorrect username");
        }
        var check = await UserManager.CheckPasswordAsync(user,changeDto.oldPassword);
        if(check is false){
            return base.BadRequest("Incorrect password");
        }
        var newUser = await UserManager.ChangePasswordAsync(user,changeDto.oldPassword,changeDto.newPassword);
        return RedirectToAction("Login");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword()
    {
        return base.View();
    }
}