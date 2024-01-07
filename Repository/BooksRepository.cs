using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SMlibraryApp.Models;

namespace SMlibraryApp.Repository;
public class BooksRepository
{
    
    private const string ConnectionString = $"Server=localhost;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;";

    public  IEnumerable<Book> GetBooks()
    {
        using var connection = new SqlConnection(ConnectionString);
        var books = connection.Query<Book>("select * from Books");
        return books;

    }

    public  Book? GetBookById(int Id)
    {

        using var connection = new SqlConnection(ConnectionString);
        var book = connection.QueryFirstOrDefault<Book>(
            sql: "select top 1 * from Books where Id = @Id",
            param: new { Id = Id });
        return book;
    }


    public  int PostBook(Book newbook)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count =  connection.Execute(
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

    public  int DeleteBook(int id)
    {

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount =  connection.Execute(
            @"delete Books
        where Id = @Id",
            param: new
            {
                Id = id,
            });

        return deletedRowsCount;
    }

}

