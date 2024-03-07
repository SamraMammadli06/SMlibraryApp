using Microsoft.AspNetCore.Identity;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Services
{
    public interface IUserService
    {
        public IEnumerable<IdentityUser> GetUsers();
        public Task Create(IdentityUser newuser);
        public Task Delete(string name);
        public Task<IEnumerable<Book>> GetMyBooks(string UserName);
        public Task<IEnumerable<Book>> GetBooksbyUser(string UserName);
        public Task<bool> AddBookToUser(int id, string UserName);
        public Task<IdentityUser?> FindUser(string UserName);
        public Task<IdentityUser> FindUserbyId(int id);
    }
}