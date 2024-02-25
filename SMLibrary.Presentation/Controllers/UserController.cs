using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMLibraryApp.Core.Repository;

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
        var userLogin = User.Identity.Name;
        var books = await userBooksRepository.GetBooksbyUser(userLogin);
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
        var userLogin = User.Identity.Name;
        if(userLogin is null)
            return BadRequest();
        await userBooksRepository.AddBookToUser(id,userLogin);

        return base.RedirectToAction("Get","Books");
    }
}
