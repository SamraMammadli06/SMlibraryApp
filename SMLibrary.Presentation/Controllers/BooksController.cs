using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
namespace SMlibraryApp.Presentation.Controllers;

public class BooksController : Controller
{
    private readonly IBookRepository repository;
    public BooksController(IBookRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Get()
    {
        var books = await repository.GetBooks();
        return View(books);
    }

    [HttpGet]

    [Authorize]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await repository.GetBookById(id);
        if (book is null)
        {
            return NotFound($"Book with id {id} not found");
        }
        return View(book);
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] Book newbook)
    {
        await repository.Create(newbook);
        return RedirectToAction("Get", "Books"); ;
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteBook(id);
        return Ok();
    }
}
