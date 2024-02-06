using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Models;
using SMlibraryApp.Repository.Base;
namespace SMlibraryApp.Controllers;

public class BooksController : Controller
{
    private readonly IBookRepository repository;
    public BooksController(IBookRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var books = await repository.GetBooks();
        return View(books);
    }

    [HttpGet]
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

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Book newbook)
    {
        var count = await repository.Create(newbook);
        if (count == 0)
        {
            return BadRequest();
        }
        return View();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var count = await repository.DeleteBook(id);
        if (count == 0)
        {
            return NotFound();
        }
        return Ok();
    }
}
