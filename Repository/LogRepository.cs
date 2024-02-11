using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using SMlibraryApp.Models;
using SMlibraryApp.Repository.Base;

namespace SMlibraryApp.Repository;
public class LogRepository : ILogRepository
{
    private readonly string ConnectionString;
    public LogRepository(string connection)
    {
        this.ConnectionString = connection;
    }

    public async Task<int> CreateLog(Log log)
    {
        using var connection = new SqlConnection(ConnectionString);
        var count = connection.ExecuteAsync(@"insert into Loggings([userId],[url],
            [methodType],[statusCode],[requestBody],[responseBody])
            values(@userId,@url,
            @methodType,@statusCode,@requestBody,@responseBody)",
            param: new
            {
                userId = log.userId,
                url = log.url,
                methodType = log.methodType,
                statusCode =  log.statusCode,
                requestBody = log.requestBody,
                responseBody = log.responseBody,
            });
        return await count;
    }
}
