using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Services;
using SMLibrary.Presentation.Dtos;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Presentation.Dtos;

namespace SMlibraryApp.Presentation.Controllers;
public class IdentityController : Controller
{
    private readonly IUserService service;
    private readonly UserManager<IdentityUser> UserManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;
    public IdentityController(IUserService service,
    UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
    RoleManager<IdentityRole> roleManager)
    {
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.UserManager = userManager;
        this.service = service;
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
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }

        var result = await signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            return RedirectToAction("Get", "Books");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(loginDto);
        }
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }

        var newUser = new IdentityUser
        {
            UserName = registerDto.UserName,
        };
        var result = await UserManager.CreateAsync(newUser, registerDto.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerDto);
        }

        if (registerDto.UserName.Contains("admin"))
        {
            var role = new IdentityRole { Name = "Admin" };
            await roleManager.CreateAsync(role);

            await UserManager.AddToRoleAsync(newUser, role.Name);
        }

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
        if (!ModelState.IsValid)
        {
            return View(changeDto);
        }

        var user = await service.FindUser(changeDto.Username);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Incorrect username");
            return View(changeDto);
        }

        var check = await UserManager.CheckPasswordAsync(user, changeDto.oldPassword);
        if (!check)
        {
            ModelState.AddModelError(string.Empty, "Incorrect password");
            return View(changeDto);
        }

        var result = await UserManager.ChangePasswordAsync(user, changeDto.oldPassword, changeDto.newPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(changeDto);
        }

        return RedirectToAction("Login");
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword()
    {
        return base.View();
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    public async Task<IActionResult> GetUsers()
    {
        var users = service.GetUsers();
        return base.View(users);
    }
}