using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository
{
    public interface IBookRepository
    {
        public Task<IEnumerable<Book>> GetBooks();
        public Task<Book?> GetBookById(int id);
        public Task Create(Book newbook);
        public Task DeleteBook(int id);
        public Task<IEnumerable<Book>> GetBooksByTag(string tag);
        public Task ChangeBook(Book book);
    }
}