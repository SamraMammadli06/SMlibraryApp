using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
namespace SMlibraryApp.Presentation.Controllers;

public class BooksController : Controller
{
    private readonly IBookServices service;
    private readonly IWebHostEnvironment webHostEnvironment;
    public BooksController(IBookServices service, IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
        this.service = service;
    }

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Get()
    {
        var books = await service.GetBooks();
        return View(books);
    }

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await service.GetBookById(id);
        return View(book);
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] Book newbook, IFormFile imageFile, IFormFile contentFile)
    {

        if (imageFile != null && imageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            string uniqueFileName1 = Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName1);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            newbook.Image = "/images/" + uniqueFileName1;
        }
        if (contentFile != null && contentFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "pdf");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Path.GetFileName(contentFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await contentFile.CopyToAsync(stream);
            }

            newbook.Content = "/pdf/" + uniqueFileName;
        }
        await service.Create(newbook);
        return RedirectToAction("Get", "Books");
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteBook(id);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("/[controller]/[action]/{id}")]
    public async Task<IActionResult> Change(int id)
    {
        var book = await service.GetBookById(id);
        return base.View(book);
    }

    [HttpGet]
    [Authorize]
    [Route("/[action]/{id}")]
    public async Task<IActionResult> ReadBook(int id)
    {
        var book = await service.GetBookById(id);
        return base.View(book);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Change(int id, [FromForm] Book book)
    {
        await service.ChangeBook(id, book);
        return RedirectToAction("Get");
    }

}
