using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Repository;

namespace SMlibraryApp.Middlewares;
public class LoggingMiddleware : IMiddleware
{
    
    LogRepository repo = new LogRepository("Server=localhost;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True;");
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var count = await repo.CreateLog(context);
        await next.Invoke(context);
    }
}
