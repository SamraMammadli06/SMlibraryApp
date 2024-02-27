using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Repository;
namespace SMlibraryApp.Presentation.Controllers;

public class BooksController : Controller
{
    private readonly IBookRepository repository;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IUserRepository UserRepository;
    public BooksController(IBookRepository repository, IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
    {
        this.UserRepository = userRepository;
        this.webHostEnvironment = webHostEnvironment;
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
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> BuyBook(int id)
    {
        var balance = await UserRepository.GetBalance(User.Identity.Name);
        var book = await repository.GetBookById(id);
        if(balance < book.Price){
            return base.BadRequest("Not enough money");
        }
        var check = await UserRepository.AddBookToUser(id,User.Identity.Name);
        if(check == false){
            await repository.BuyBook(id, User.Identity.Name);
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
        await repository.Create(newbook);
        return RedirectToAction("Get", "Books");
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteBook(id);
        return Ok();
    }


    [HttpGet]
    [Authorize]
    [Route("/[action]/{id}")]
    public async Task<IActionResult> ReadBook(int id)
    {
        var book = await repository.GetBookById(id);
        return base.View(book);
    }

}
