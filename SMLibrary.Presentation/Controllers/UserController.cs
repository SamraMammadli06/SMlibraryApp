using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Repository;

namespace SMLibrary.Presentation.Controllers;


public class UserController : Controller
{
    private readonly IUserRepository userBooksRepository;
    public UserController(IUserRepository userBooksRepository)
    {
        this.userBooksRepository = userBooksRepository;
    }
    [HttpGet]
    [Authorize]
    [Route("/[controller]/Books")]
    public async Task<IActionResult> GetUserItems()
    {
        var userLogin = User.Identity.Name;
        var books = await userBooksRepository.GetBooksbyUser(userLogin);
        return base.View(books);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Account()
    {
        return base.View();
    }


    [HttpPost]
    [Authorize]
    [Route("[controller]/add/{id}")]
    public async Task<IActionResult> AddBookToUser(int id)
    {
        var userLogin = User.Identity.Name;
        if (userLogin is null)
            return BadRequest();
        await userBooksRepository.AddBookToUser(id, userLogin);

        return base.RedirectToAction("Get", "Books");
    }

}
