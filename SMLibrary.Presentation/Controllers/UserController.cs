using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SMLibrary.Presentation.Controllers;
public class UserController : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Account(){
        var claims = base.User.Claims;
        return base.View(model: claims);
    }
}
