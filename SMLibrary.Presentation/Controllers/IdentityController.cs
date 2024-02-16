using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Presentation.Dtos;

namespace SMlibraryApp.Presentation.Controllers;
public class IdentityController : Controller
{
    private readonly IDataProtector dataProtector;
    private readonly IUserRepository userRepository;

    public IdentityController(IUserRepository userRepository, IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtector = dataProtectionProvider.CreateProtector("Key");
        this.userRepository = userRepository;
    }
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return base.View();
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return base.View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        var user = await userRepository.FindUser(new User()
        {
            UserName = loginDto.UserName,
            Password = loginDto.Password,
        });
        if (user is null)
            return base.BadRequest("Incorrect user information");

        var userHash = this.dataProtector.Protect(user.Id.ToString());
        base.HttpContext.Response.Cookies.Append("Authorize", userHash);
        return RedirectToAction("Get", "Books");
    }

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
}