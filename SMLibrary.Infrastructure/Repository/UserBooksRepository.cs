using System.Data.SqlClient;
using Dapper;
using SMLibrary.Core.Repository;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Infrastructure.Repository;

public class UserBooksRepository : IUserBooksRepository
{
    private readonly string ConnectionString;
    public UserBooksRepository(string connection)
    {
        this.ConnectionString = connection;
    }
    public async Task<IEnumerable<Book>> GetBooksbyUser(string UserName, string Email)
    {

        using var connection = new SqlConnection(ConnectionString);
        var users = await connection.QueryAsync<Book>(@"SELECT b.Id, b.Author,b.Name,b.Price
                    FROM Users u JOIN Books b ON (u.BookId = b.Id) where u.UserName = @name and u.Email = @email",
                    param: new
                    {
                        name = UserName,
                        email = Email
                    });
        return users;
    }
    public async Task<User> FindUserByEmailandName(User user)
    {
        using var connection = new SqlConnection(ConnectionString);
        var u = await connection.QueryFirstOrDefaultAsync<User>(@"select * from Users
            where UserName = @UserName and Email = @Email",
            param: new
            {
                user.UserName,
                user.Email,
            });
        return u;
    }
    public async Task AddBookToUser(int id, User user)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count = await connection.ExecuteAsync(@"insert into Users (Email, Password,UserName,BookId) 
            values(@Email, @Password,@UserName,@BookId)",
            param: new
            {
                BookId = id,
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName
            });
    }
}
