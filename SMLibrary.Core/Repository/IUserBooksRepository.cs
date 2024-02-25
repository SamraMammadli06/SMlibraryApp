using System.Security.Claims;
using SMlibraryApp.Core.Models;

namespace SMLibraryApp.Core.Repository;
public interface IUserBooksRepository
{
    public Task<IEnumerable<Book>> GetBooksbyUser(string UserName);
    public Task AddBookToUser(int id, string UserName);
}
