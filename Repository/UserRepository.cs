using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dapper;
using SMlibraryApp.Models;
using SMlibraryApp.Repository.Base;

namespace SMlibraryApp.Repository;
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
        var count = connection.ExecuteAsync(@"insert into Users (Email, Password,UserName) 
            values(@Email, @Password,@UserName)",
            param: new
            {
                newuser.Email,
                newuser.Password,
                newuser.UserName,
            });
        return await count;
    }

    public async Task<IEnumerable<User>> Get()
    {
        using var connection = new SqlConnection(ConnectionString);
        var users = connection.QueryAsync<User>("select * from Users");
        return await users;
    }

    public async Task<User> FindUser(User user)
    {
        using var connection = new SqlConnection(ConnectionString);
        var u = connection.QueryFirstOrDefaultAsync<User>(@"select * from Users
            where UserName = @UserName and Password = @Password",
            param: new
            {
                user.UserName,
                user.Password,
            });
        return await u;
    }
}
