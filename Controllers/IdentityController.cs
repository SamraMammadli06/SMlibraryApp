using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Dtos;

namespace SMlibraryApp.Controllers;
public class IdentityController :Controller
{
    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl){
        base.ViewData["returnUrl"] = returnUrl;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto){
        var claims = new Claim[] {
            new Claim(ClaimTypes.Name, loginDto.UserName),
            new Claim("creation_date_utc", DateTime.UtcNow.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await base.HttpContext.SignInAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
            principal: new ClaimsPrincipal(claimsIdentity)
        );

        if(string.IsNullOrWhiteSpace(loginDto.ReturnUrl)) 
            return base.RedirectToAction(controllerName: "Books", actionName: "Get");

        return base.RedirectPermanent(loginDto.ReturnUrl);
    }
}