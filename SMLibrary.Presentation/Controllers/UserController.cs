using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Repository;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Presentation.Controllers;
[Authorize]
public class UserController : Controller
{
    private readonly IUserBooksRepository userBooksRepository;
    public UserController(IUserBooksRepository userBooksRepository)
    {
        this.userBooksRepository = userBooksRepository;
    }
    [HttpGet]
    [Route("/[controller]/Books")]
    public async Task<IActionResult> GetUserItems()
    {
        var email = User.FindFirst(ClaimTypes.Email).Value;
        var name = User.FindFirst(ClaimTypes.Name).Value;
        var books = await userBooksRepository.GetBooksbyUser(name, email);
        return base.View(books);
    }
    [HttpGet]
    public async Task<IActionResult> Account()
    {
        return base.View();
    }


    [HttpPost]
    [Route("[controller]/add/{id}")]
    public async Task<IActionResult> AddBookToUser(int id)
    {
        var user = new User()
        {
            Email = User.FindFirst(ClaimTypes.Email).Value,
            UserName = User.FindFirst(ClaimTypes.Name).Value,
        };
        user = await userBooksRepository.FindUserByEmailandName(user);
        await userBooksRepository.AddBookToUser(id, user);
        return RedirectToAction("Get","Books");
    }
}
