using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Models;
using SMlibraryApp.Repository.Base;
using SMlibraryApp.Services.Base;
namespace SMlibraryApp.Controllers;

public class BooksController : Controller
{
    private readonly IBookRepository repository;
    private readonly IIdentityService Identity;
    public BooksController(IBookRepository repository, IIdentityService identity)
    {
        this.Identity = identity;
        this.repository = repository;
    }

    [HttpGet]
    [Route("Get/")]
    public async Task<IActionResult> Get()
    {
        var isUserExists = this.Identity.IUserExsists(base.HttpContext);
        if (!isUserExists)
        {
            return base.Unauthorized();
        }
        var books = await repository.GetBooks();
        return View(books);
    }

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var isUserExists = this.Identity.IUserExsists(base.HttpContext);
        if (!isUserExists)
        {
            return base.Unauthorized();
        }
        var book = await repository.GetBookById(id);
        if (book is null)
        {
            return NotFound($"Book with id {id} not found");
        }
        return View(book);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Book newbook)
    {
        var isUserExists = this.Identity.IUserExsists(base.HttpContext);
        if (!isUserExists)
        {
            return base.Unauthorized();
        }
        var count = await repository.Create(newbook);
        if (count == 0)
        {
            return BadRequest();
        }
        return RedirectToAction("Get", "Books"); ;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var isUserExists = this.Identity.IUserExsists(base.HttpContext);
        if (!isUserExists)
        {
            return base.Unauthorized();
        }
        var count = await repository.DeleteBook(id);
        if (count == 0)
        {
            return NotFound();
        }
        return Ok();
    }
}
