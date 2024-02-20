using System.Data.SqlClient;
using Dapper;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;

namespace SMlibraryApp.Infrastructure.Repository;
public class UserRepository : IUserRepository
{
    private readonly string ConnectionString;
    public UserRepository(string ConnectionString)
    {
        this.ConnectionString = ConnectionString;
    }

    public async Task<int> Create(User newuser)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count = await connection.ExecuteAsync(@"insert into Users (Email, Password,UserName) 
            values(@Email, @Password,@UserName)",
            param: new
            {
                newuser.Email,
                newuser.Password,
                newuser.UserName,
            });
        return count;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        using var connection = new SqlConnection(ConnectionString);
        var users = await connection.QueryAsync<User>("select * from Users");
        return users;
    }

    public async Task<User?> FindUser(User user)
    {
        using var connection = new SqlConnection(ConnectionString);
        var u = await connection.QueryFirstOrDefaultAsync<User>(@"select * from Users
            where UserName = @UserName and Password = @Password",
            param: new
            {
                user.UserName,
                user.Password,
            });
        return u;
    }
}
