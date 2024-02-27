using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Presentation.Dtos;
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
        var balance = await userBooksRepository.GetBalance(User.Identity.Name);
        return base.View(balance);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddBalance([FromForm] CardDto card){
        if(card.Amount<=0 || card.CVV.Length!=3){
            return BadRequest("Wrong Entered data");
        }
        var CardNumbers = card.CardNumber.Split(" ");
        if(CardNumbers.Length==4 || CardNumbers.Length==1){
            await userBooksRepository.SetBalance(card.Amount,User.Identity.Name);
            return base.RedirectToAction("Account");
        }
        return BadRequest("Wrong Entered ");
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> AddBalance(){
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
