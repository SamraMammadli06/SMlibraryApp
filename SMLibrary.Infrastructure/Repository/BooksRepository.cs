using System.Data.SqlClient;
using Dapper;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
namespace SMlibraryApp.Infrastructure.Repository;

public class BooksRepository : IBookRepository
{
    private readonly string ConnectionString;
    public BooksRepository(string connection)
    {
        this.ConnectionString = connection;
    }
    public async Task<IEnumerable<Book>> GetBooks()
    {
        using var connection = new SqlConnection(ConnectionString);
        var books = await connection.QueryAsync<Book>("select * from Books");
        return books;
    }

    public async Task<Book?> GetBookById(int Id)
    {
        using var connection = new SqlConnection(ConnectionString);
        var book = await connection.QueryFirstOrDefaultAsync<Book>(
            sql: "select top 1 * from Books where Id = @Id",
            param: new { Id = Id });
        return book;
    }

    public async Task<int> Create(Book newbook)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count = await connection.ExecuteAsync(
            @"insert into Books (Name, Author,Price) 
        values(@Name, @Author,@Price)",
            param: new
            {
                newbook.Name,
                newbook.Author,
                newbook.Price,
            });
        return count;
    }

    public async Task<int> DeleteBook(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Books
        where Id = @Id",
            param: new
            {
                Id = id,
            });

        return deletedRowsCount;
    }
}

