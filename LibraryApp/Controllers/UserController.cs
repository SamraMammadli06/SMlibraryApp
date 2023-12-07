namespace LibraryApp.Controllers;

using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using LibraryApp.Controllers.Base;
using LibraryApp.Models;
using LibraryApp.Extensions;
using Dapper;
using LibraryApp.Attributes;

public class UserController : ControllerBase
{
     private const string ConnectionString = $"Server=localhost;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;";

    [HttpGet("GetAll")]
    public async Task GetUsersAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var users = await connection.QueryAsync<User>("select * from Users");

        var usersHtml = users.GetHtml();
        await writer.WriteLineAsync(usersHtml);
        context.Response.ContentType = "text/html";

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    public async Task GetUserByIdAsync(HttpListenerContext context)
    {
        var userIdToGetObj = context.Request.QueryString["id"];

        if (userIdToGetObj == null || int.TryParse(userIdToGetObj, out int userIdToGet) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            sql: "select top 1 * from Users where Id = @Id",
            param: new { Id = userIdToGet });

        if (user is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(user));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }



    [HttpPost("Create")]
    public async Task PostuserAsync(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newuser = JsonSerializer.Deserialize<User>(json);

        if (newuser == null || newuser.FullName == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var users = await connection.ExecuteAsync(
            @"insert into Users (FullName, Email) 
        values(@FullName, @Email)",
            param: new
            {
                newuser.FullName,
                newuser.Email,
            });

        context.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    public async Task DeleteUserAsync(HttpListenerContext context)
    {
        var userIdToDeleteObj = context.Request.QueryString["id"];

        if (userIdToDeleteObj == null || int.TryParse(userIdToDeleteObj, out int userIdToDelete) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var deletedRowsCount = await connection.ExecuteAsync(
            @"delete Users
        where Id = @Id",
            param: new
            {
                Id = userIdToDelete,
            });

        if (deletedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    public async Task PutUserAsync(HttpListenerContext context)
    {
        var userIdToUpdateObj = context.Request.QueryString["id"];

        if (userIdToUpdateObj == null || int.TryParse(userIdToUpdateObj, out int userIdToUpdate) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var userToUpdate = JsonSerializer.Deserialize<User>(json);

        if (userToUpdate == null || string.IsNullOrEmpty(userToUpdate.FullName))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var affectedRowsCount = await connection.ExecuteAsync(
            @"update Users
        set FullName = @FullName, Email = @Email
        where Id = @Id",
            param: new
            {
                userToUpdate.FullName,
                userToUpdate.Email,
                Id = userIdToUpdate
            });

        if (affectedRowsCount == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}