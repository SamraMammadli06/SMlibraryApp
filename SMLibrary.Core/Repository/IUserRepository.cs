using SMlibraryApp.Core.Models;

namespace SMlibraryApp.Core.Repository;
public interface IUserRepository
{
    public Task<IEnumerable<User>> Get();
    public Task<int> Create(User newuser);
    public Task<User?> FindUser(User user);
}
