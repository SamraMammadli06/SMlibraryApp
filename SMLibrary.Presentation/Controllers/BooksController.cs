using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
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
        ViewBag.MyBooks = await userService.GetBooksbyUser(User.Identity.Name);
        return View(books);
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try{
            var book = await service.GetBookById(id);
            if (book is null || book.Id==0)
            {
                return base.NotFound($"Book with id {id} not found");
            }
            
            var comments = await service.GetComments(id);
            ViewBag.Comments = comments;
            return View(book);
        }
        catch(Exception ){
            return base.NotFound($"Book with id {id} not found");
        }
    }
    
    [HttpGet]
    [Route("[controller]/[action]/{tag}")]
    public async Task<IActionResult> ByTag(string tag)
    {
        var books = await service.GetBooksByTag(tag);
        ViewBag.MyBooks = await userService.GetBooksbyUser(User.Identity.Name);
        return View(books);
    }

    [HttpGet]
    [Authorize]
    [Route("/[action]/{id}")]
    public async Task<IActionResult> ChangeBook(int id)
    {
        var book = await service.GetBookById(id);
        return View(book);
    }

    [HttpPost]
    [Authorize]
    [Route("/[action]/{author}/{id}")]
    public async Task<IActionResult> AddComment([FromForm] string comment,string author,int id)
    {
        await service.AddComment(author,comment,User.Identity.Name);
        return base.RedirectToAction("GetById","Books",new{id = id});
    }

    [HttpPost]
    [Authorize]
    [Route("/[action]/{id}")]
    public async Task<IActionResult> ChangeBook([FromForm] Book newbook, IFormFile imageFile)
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

        await service.ChangeBook(newbook);
        return RedirectToAction("Account", "User");
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
        return RedirectToAction("Account", "User");
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
        newbook.IsFinished=true;
        await service.Create(newbook);
        return base.RedirectToAction("Get", "Books");
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
