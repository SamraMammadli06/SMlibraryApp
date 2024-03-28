using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMLibrary.Core.Models;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Services
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book?> GetBookById(int id);
        public Task Create(Book newbook);
        public Task DeleteBook(int id);
        public Task<IEnumerable<Book>> GetBooksByTag(string tag);
        public Task ChangeBook(Book book);
        public Task AddComment(string author,string comment,string userName);
        public Task<IEnumerable<Comment>> GetComments(int id);
    }
}