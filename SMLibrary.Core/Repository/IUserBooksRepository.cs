using System.Security.Claims;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Repository;
public interface IUserBooksRepository
{
    public Task<IEnumerable<Book>> GetBooksbyUser(string UserName,string Email);
    public Task AddBookToUser(int id, User user);
}
