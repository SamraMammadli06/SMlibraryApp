namespace LibraryApp.Controllers;

using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using LibraryApp.Controllers.Base;
using LibraryApp.Models;
using LibraryApp.Extensions;
using Dapper;
using LibraryApp.Attributes;

public class BookController : ControllerBase
{
     private const string ConnectionString = $"Server=localhost;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;";

    [HttpGet("GetAll")]
    public async Task GetBooksAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var books = await connection.QueryAsync<Book>("select * from Books");

        var booksHtml = books.GetHtml();
        await writer.WriteLineAsync(booksHtml);
        context.Response.ContentType = "text/html";

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetBookByIdAsync(HttpListenerContext context)
    {
        var bookIdToGetObj = context.Request.QueryString["id"];

        if (bookIdToGetObj == null || int.TryParse(bookIdToGetObj, out int bookIdToGet) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var book = await connection.QueryFirstOrDefaultAsync<Book>(
            sql: "select top 1 * from Books where Id = @Id",
            param: new { Id = bookIdToGet });

        if (book is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(book));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }



    [HttpPost("Create")]
    public async Task PostBookAsync(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newbook = JsonSerializer.Deserialize<Book>(json);

        if (newbook == null || newbook.Price == null || string.IsNullOrWhiteSpace(newbook.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var books = await connection.ExecuteAsync(
            @"insert into Books (Name, Price) 
        values(@Name, @Price)",
            param: new
            {
                newbook.Name,
                newbook.Price,
            });

        context.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteBookAsync(HttpListenerContext context)
    {
        var bookIdToDeleteObj = context.Request.QueryString["id"];

        if (bookIdToDeleteObj == null || int.TryParse(bookIdToDeleteObj, out int bookIdToDelete) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Books
        where Id = @Id",
            param: new
            {
                Id = bookIdToDelete,
            });

        if (deletedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutBookAsync(HttpListenerContext context)
    {
        var bookIdToUpdateObj = context.Request.QueryString["id"];

        if (bookIdToUpdateObj == null || int.TryParse(bookIdToUpdateObj, out int bookIdToUpdate) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var bookToUpdate = JsonSerializer.Deserialize<Book>(json);

        if (bookToUpdate == null || bookToUpdate.Price == null || string.IsNullOrEmpty(bookToUpdate.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Books
        set Name = @Name, Price = @Price
        where Id = @Id",
            param: new
            {
                bookToUpdate.Name,
                bookToUpdate.Price,
                Id = bookIdToUpdate
            });

        if (affectedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}