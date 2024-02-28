using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Services
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book?> GetBookById(int id);
        public Task Create(Book newbook);
        public Task DeleteBook(int id);
        public Task BuyBook(int id, string UserName);
    }
}