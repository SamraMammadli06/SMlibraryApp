using Microsoft.AspNetCore.Identity;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Services
{
    public interface IUserService
    {
        public IEnumerable<IdentityUser> GetUsers();
        public Task Create(IdentityUser newuser);
        public Task Delete(int id);
        public Task<IEnumerable<Book>> GetBooksbyUser(string UserName);
        public Task<bool> AddBookToUser(int id, string UserName);
        public Task<IdentityUser?> FindUser(string UserName);
        public Task<IdentityUser> FindUserbyId(int id);
        public Task SetBalance(double amount, string UserName);
        public Task<double> GetBalance(string UserName);
        public Task AddBalancetoUser(string UserName);
    }
}