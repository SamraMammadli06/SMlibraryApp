using System.Data.SqlClient;
using Dapper;
using SMlibraryApp.Repository.Base;

namespace SMlibraryApp.Repository;
public class LogRepository : ILogRepository
{
    private readonly string ConnectionString;
    public LogRepository(string connection)
    {
        this.ConnectionString = connection;
    }

    public async Task<int> CreateLog(HttpContext context)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count = connection.ExecuteAsync(@"insert into Loggings([userId],[url],
            [methodType],[statusCode],[requestBody],[responseBody])
            values(@userId,@url,
            @methodType,@statusCode,@requestBody,@responseBody)",
            param: new
            {
                userId = 1,
                url = context.Request.Path.ToString(),
                methodType = context.Request.Method.ToString(),
                statusCode = context.Response.StatusCode,
                requestBody = context.Request.Body.ToString(),
                responseBody = context.Response.Body.ToString(),
            });
        return await count;
    }
}
