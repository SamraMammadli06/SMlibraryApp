using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Services;
using SMLibrary.Presentation.Dtos;
using SMlibraryApp.Core.Repository;

namespace SMLibrary.Presentation.Controllers;


public class UserController : Controller
{
    private readonly IUserService service;
    public UserController(IUserService service)
    {
        this.service = service;
    }
    [HttpGet]
    [Authorize(Roles ="Admin")]
    [Route("/[controller]/Books")]
    public async Task<IActionResult> GetUserItems()
    {
        var userLogin = User.Identity.Name;
        var books = await service.GetBooksbyUser(userLogin);
        return base.View(books);
    }

    [HttpDelete]
    [Authorize(Roles ="Admin")]
    [Route("/[controller]/[action]/{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await service.Delete(name);
        return base.Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Account()
    {
        var balance = await service.GetBalance(User.Identity.Name);
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
            await service.SetBalance(card.Amount,User.Identity.Name);
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
        await service.AddBookToUser(id, userLogin);

        return base.RedirectToAction("Get", "Books");
    }

}
