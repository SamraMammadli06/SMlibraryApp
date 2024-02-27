using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Services;
public interface IBookServices
{
    public Task<IEnumerable<Book>> GetBooks();
    public Task<Book?> GetBookById(int id);
    public Task Create(Book newbook);
    public Task DeleteBook(int id);
    public Task ChangeBook(int id, Book newbook);
}
