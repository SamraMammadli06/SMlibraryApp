using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SMlibraryApp.Models;
using SMlibraryApp.Repository.Base;
namespace SMlibraryApp.Repository;

public class BooksRepository : IBookRepository
{
    
    private readonly string ConnectionString;
    public BooksRepository(string connection){
        this.ConnectionString = connection;
    }
    public  async Task<IEnumerable<Book>> GetBooks()
    {
        using var connection = new SqlConnection(ConnectionString);
        var books = connection.QueryAsync<Book>("select * from Books");
        return await books;

    }

    public async Task<Book?> GetBookById(int Id)
    {
        using var connection = new SqlConnection(ConnectionString);
        var book = connection.QueryFirstOrDefaultAsync<Book>(
            sql: "select top 1 * from Books where Id = @Id",
            param: new { Id = Id });
        return await book;
    }


    public async Task<int> Create(Book newbook)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count =  connection.ExecuteAsync(
            @"insert into Books (Name, Author,Price) 
        values(@Name, @Author,@Price)",
            param: new
            {
                newbook.Name,
                newbook.Author,
                newbook.Price,
            });
        return await count;
    }

    public  async Task<int> DeleteBook(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount =  connection.ExecuteAsync(
            @"delete Books
        where Id = @Id",
            param: new
            {
                Id = id,
            });

        return await deletedRowsCount;
    }
}

