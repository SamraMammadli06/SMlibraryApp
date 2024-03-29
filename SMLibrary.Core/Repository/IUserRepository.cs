using Microsoft.AspNetCore.Identity;
using SMLibrary.Core.Models;
using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository;
public interface IUserRepository
{
    public IEnumerable<IdentityUser> GetUsers();
    public Task Create(IdentityUser newuser);
    public Task Delete(string name);
    public Task<IEnumerable<Book>> GetBooksbyUser(string UserName);
    public Task<IEnumerable<Book>> GetMyBooks(string UserName);
    public Task<bool> AddBookToUser(int id, string UserName);
    public Task<IdentityUser?> FindUser(string UserName);
    public Task<IdentityUser> FindUserbyId(int id);
    public  Task Edit(UserCustomUser customUser,string name);
    public Task<UserCustomUser> GetCustomUser(string UserName);
    public Task CreateCustomUser(UserCustomUser customUser);
    public Task<UserCustomUser> GetUser(string UserName);
    public Task DeleteBookbyUser(string UserName, int Id);
    public Task<IEnumerable<Comment>> GetMyComments(string UserName);
}
