using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository;
public interface IUserRepository
{
    public Task<IEnumerable<User>> GetUsers();
    public Task<int> Create(User newuser);
    public Task<User?> FindUser(User user);
}
