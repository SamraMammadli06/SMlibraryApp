using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Models;
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
    [Authorize]
    [Route("/[controller]/Books")]
    public async Task<IActionResult> GetUserItems()
    {
        var userLogin = User.Identity.Name;
        var books = await service.GetBooksbyUser(userLogin);
        return base.View(books);
    }

    [HttpDelete]
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
        var books = await service.GetMyBooks(User.Identity.Name);
        ViewBag.customUser =  await service.GetCustomUser(User.Identity.Name);
        return base.View(books);
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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var user = await service.GetCustomUser(User.Identity.Name);
        return View(user);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit([FromForm] UserCustomUser customUser)
    {
        await service.Edit(customUser);
        return RedirectToAction("Account");
    }

}
