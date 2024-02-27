using Microsoft.AspNetCore.Identity;
using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository;
public interface IUserRepository
{
    public IEnumerable<IdentityUser> GetUsers();
    public Task Create(IdentityUser newuser);
    public Task Delete(int id);
    public Task<IEnumerable<Book>> GetBooksbyUser(string UserName);
    public Task AddBookToUser(int id, string UserName);
    public Task<IdentityUser?> FindUser(string UserName);
    public Task<IdentityUser> FindUserbyId(int id);
}
