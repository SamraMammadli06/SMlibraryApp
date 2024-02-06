using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using SMlibraryApp.Models;
using SMlibraryApp.Repository.Base;
using SMlibraryApp.Services.Base;

namespace SMlibraryApp.Middlewares;
public class LoggingMiddleware : IMiddleware
{
    private readonly ILogRepository Repository;
    private readonly IDataProtector dataProtector;
    private readonly ILogService logService;

    public LoggingMiddleware(ILogRepository repository, IDataProtectionProvider dataProtection, ILogService logService)
    {
        this.logService = logService;
        this.dataProtector = dataProtection.CreateProtector("Key");
        this.Repository = repository;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!logService.IsLogEnabled())
        {
            await next.Invoke(context);
        }
        else
        {
            var requestBody = string.Empty;

            if (context.Request.Body.CanRead)
            {
                if (!context.Request.Body.CanSeek)
                {
                    context.Request.EnableBuffering();
                }

                context.Request.Body.Position = 0;

                StreamReader requestReader = new(context.Request.Body, Encoding.UTF8);

                requestBody = await requestReader.ReadToEndAsync();

                context.Request.Body.Position = 0;
            }

            var originalBodyStream = context.Response.Body;
            var body = string.Empty;

            using (var memoryBodyStream = new MemoryStream())
            {

                context.Response.Body = memoryBodyStream;

                await next.Invoke(context);

                memoryBodyStream.Seek(0, SeekOrigin.Begin);
                body = await new StreamReader(memoryBodyStream).ReadToEndAsync();
                memoryBodyStream.Seek(0, SeekOrigin.Begin);

                await memoryBodyStream.CopyToAsync(originalBodyStream);

            }

            context.Response.Body = originalBodyStream;

            var userId = 0;
            if (context.Request.Cookies["Authorize"] is not null)
                userId = Convert.ToInt32(dataProtector.Unprotect(context.Request.Cookies["Authorize"]));

            var count = await Repository.CreateLog(new Log()
            {
                userId = userId,
                url = context.Request.GetDisplayUrl(),
                methodType = context.Request.Method,
                statusCode = context.Response.StatusCode,
                requestBody = requestBody,
                responseBody = body,
            });
        }
    }
}
