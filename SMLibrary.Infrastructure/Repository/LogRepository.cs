using System.Data.SqlClient;
using Dapper;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;

namespace SMlibraryApp.Infrastructure.Repository;
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
        var count = await connection.ExecuteAsync(@"insert into Loggings([userId],[url],
            [methodType],[statusCode],[requestBody],[responseBody])
            values(@userId,@url,
            @methodType,@statusCode,@requestBody,@responseBody)",
            param: log);
        return count;
    }
}
