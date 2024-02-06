using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Repository;
using SMlibraryApp.Repository.Base;

namespace SMlibraryApp.Middlewares;
public class LoggingMiddleware : IMiddleware
{
    public ILogRepository Repository { get; }

    public LoggingMiddleware(ILogRepository repository)
    {
        this.Repository = repository;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var count = await Repository.CreateLog(context);
        await next.Invoke(context);
    }
}
