using System.Data.SqlClient;
using Dapper;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Data;

namespace SMlibraryApp.Infrastructure.Repository;
public class LogRepository : ILogRepository
{
    private readonly MyDbContext dbContext;

    public LogRepository(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateLog(Log log)
    {
        await this.dbContext.Logs.AddAsync(log);
        await this.dbContext.SaveChangesAsync();
    }
}
