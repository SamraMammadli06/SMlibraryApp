using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Repository;
namespace SMlibraryApp.Presentation.Controllers;

public class BooksController : Controller
{
    private readonly IBookService service;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IUserService userService;
    public BooksController(IBookService service, IUserService userService, IWebHostEnvironment webHostEnvironment)
    {
        this.userService = userService;
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
        if (book is null)
        {
            return NotFound($"Book with id {id} not found");
        }
        return View(book);
    }
    [HttpGet]
    [Route("[controller]/[action]/{tag}")]
    public async Task<IActionResult> ByTag(string tag)
    {
        var books = await service.GetBooksByTag(tag);

        return View(books);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Write()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Write([FromForm] Book newbook, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            string uniqueFileName1 = Path.GetFileName(imageFile.FileName);
            string filePathh = Path.Combine(uploadsFolder, uniqueFileName1);

            using (var stream = new FileStream(filePathh, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            newbook.Image = "/images/" + uniqueFileName1;
        }
        else
        {
            newbook.Image = "/images/" + "notFound.png";
        }
        if (!string.IsNullOrEmpty(newbook.Content))
        {
            string filesFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Files");

            if (!Directory.Exists(filesFolder))
            {
                Directory.CreateDirectory(filesFolder);
            }

            string textFileName = $"{newbook.Name}-{newbook.Author}.txt";
            string textFilePath = Path.Combine(filesFolder, textFileName);

            System.IO.File.WriteAllText(textFilePath, newbook.Content);
            newbook.Content = textFilePath;
        }


        await service.Create(newbook);
        return RedirectToAction("Get", "Books");
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
        else
        {
            newbook.Image = "/images/" + "notFound.png";
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
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteBook(id);
        return Ok();
    }


    [HttpGet]
    [Authorize]
    [Route("/[action]/{id}")]
    public async Task<IActionResult> ReadBook(int id)
    {
        var book = await service.GetBookById(id);
        return base.View(book);
    }

}
