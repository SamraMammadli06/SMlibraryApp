using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Presentation.Dtos;
using SMLibraryApp.Core.Repository;

namespace SMLibrary.Presentation.Controllers;


public class UserController : Controller
{
    private readonly IUserBooksRepository userBooksRepository;
    public UserController(IUserBooksRepository userBooksRepository)
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

    [HttpGet]
    // [Authorize(Roles = "admin")]
    [Route("[controller]")]
    public async Task<IActionResult> GetUsers()
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
