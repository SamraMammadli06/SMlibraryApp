using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Models;

namespace SMlibraryApp.Repository.Base
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book?> GetBookById(int id);
        public Task<int> Create(Book newbook);
        public Task<int> DeleteBook(int id);
    }
}