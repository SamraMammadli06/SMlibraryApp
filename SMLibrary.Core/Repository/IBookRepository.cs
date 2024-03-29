using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book?> GetBookById(int id);
        public Task<int> Create(Book newbook);
        public Task<int> DeleteBook(int id);
    }
}