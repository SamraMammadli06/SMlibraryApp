using Microsoft.AspNetCore.Identity;
using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository;
public interface IUserRepository
{
    public IEnumerable<IdentityUser> GetUsers();
    public Task Create(IdentityUser newuser);
    public Task<IdentityUser?> FindUser(string UserName);
}
