using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Models;
using SMLibrary.Core.Services;
namespace SMLibrary.Presentation.Controllers;

public class UserController : Controller
{
    private readonly IUserService service;
    private readonly IWebHostEnvironment webHostEnvironment;
    public UserController(IUserService service, IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
        this.service = service;
    }
    [HttpGet]
    [Authorize]
    [Route("/[controller]/Books")]
    public async Task<IActionResult> GetUserItems()
    {
        var userLogin = User.Identity.Name;
        var books = await service.GetBooksbyUser(userLogin);
        ViewBag.MyBooks = await service.GetBooksbyUser(User.Identity.Name);
        return base.View(books);
    }

    [HttpDelete]
    [Authorize]
    [Route("/[controller]/[action]/{name}")]
    public async Task<IActionResult> Delete(string name)
    {
        await service.Delete(name);
        
        return base.Ok();
    }

    [HttpDelete]
    [Authorize]
    [Route("/[controller]/deleteBook/{id}")]
    public async Task<IActionResult> DeleteBookbyUser(int id)
    {
        await service.DeleteBookbyUser(User.Identity.Name, id);
        return base.Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Account()
    {
        var books = await service.GetMyBooks(User.Identity.Name);
        ViewBag.customUser = await service.GetCustomUser(User.Identity.Name);
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

        return base.Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        try
        {
            var user = await service.GetCustomUser(User.Identity.Name);
            return View(user);
        }
        catch (Exception)
        {
            return base.BadRequest("Something Went Wrong");
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/[action]/{name}")]
    public async Task<IActionResult> GetUser(string name)
    {
        try
        {
            var user = await service.GetUser(name);
            ViewBag.Books = await service.GetMyBooks(name);
            ViewBag.Comments = await service.GetMyComments(name);
            ViewBag.MyBooks = await service.GetBooksbyUser(User.Identity.Name);
            return View(user);
        }
        catch (Exception)
        {
            return base.NotFound("This User is not found");
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit([FromForm] UserCustomUser customUser, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "user");
            string uniqueFileName1 = Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName1);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            customUser.ImageUrl = "/user/" + uniqueFileName1;
        }
        await service.Edit(customUser, User.Identity.Name);
        return RedirectToAction("Account");
    }

}
