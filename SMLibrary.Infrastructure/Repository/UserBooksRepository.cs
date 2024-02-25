using System.Data.SqlClient;
using Dapper;
using SMLibrary.Core.Repository;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Infrastructure.Data;

namespace SMLibrary.Infrastructure.Repository;

public class UserBooksRepository : IUserBooksRepository
{
    private readonly MyDbContext dbContext;

    public UserBooksRepository(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task AddBookToUser(int id, User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> FindUserByEmailandName(User user)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> GetBooksbyUser(string UserName, string Email)
    {
        throw new NotImplementedException();
    }
}
