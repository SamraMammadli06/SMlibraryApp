using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Models;

namespace SMlibraryApp.Repository.Base
{
    public interface IRepository
    {
        public  IEnumerable<Book> GetBooks();
        public Book? GetBookById(int id);
        public int PostBook(Book newbook);
        public int DeleteBook(int id);
    }
}