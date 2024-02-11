using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Models;

namespace SMlibraryApp.Repository.Base;
public interface IUserRepository
{
    public Task<IEnumerable<User>> Get();
    public Task<int> Create(User newuser);
    public Task<User?> FindUser(User user);
}
