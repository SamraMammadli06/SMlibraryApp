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
    [Authorize]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BuyBook(int id)
    {
        var balance = await userService.GetBalance(User.Identity.Name);
        var book = await service.GetBookById(id);
        if (balance < book.Price)
        {
            return base.BadRequest("Not enough money");
        }
        var check = await userService.AddBookToUser(id, User.Identity.Name);
        if (check == false)
        {
            await service.BuyBook(id, User.Identity.Name);
        }
        return RedirectToAction("Get");
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
    [Authorize]
    [Route("/[action]/{id}")]
    public async Task<IActionResult> ReadBook(int id)
    {
        var book = await service.GetBookById(id);
        return base.View(book);
    }

}
