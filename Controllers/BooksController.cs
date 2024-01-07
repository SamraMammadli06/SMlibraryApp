using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SMlibraryApp.Models;
using SMlibraryApp.Repository;

namespace SMlibraryApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksRepository repository = new BooksRepository();
       [HttpGet]
        public IActionResult GetBooks()
        {
            var books = repository.GetBooks();
            if(books is null){
                return NotFound("Nothing Found");
            }
            return Ok(books);
        }
        [HttpGet]
        public IActionResult GetBookById(int id)
        {
            var book = repository.GetBookById(id);
            if(book is null){
                return NotFound("Book with this id not found");
            }
            return Ok(book);
        }
        [HttpPost]
        public IActionResult PostBook(Book newbook){
            var count = repository.PostBook(newbook);
            if(count==0){
                return StatusCode(505);
            }
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteBook(int id){
            var count =  repository.DeleteBook(id);
            if(count==0){
                return NotFound();
            }
            return Ok();
        }
    }
}