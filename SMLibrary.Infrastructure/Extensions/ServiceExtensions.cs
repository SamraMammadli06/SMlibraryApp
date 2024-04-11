using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMLibrary.Core.Services;
using SMLibrary.Infrastructure.Services;
using SMlibraryApp.Core.Services;
using SMlibraryApp.Infrastructure.Services;

namespace SMLibrary.Infrastructure.Extensions;
public static class ServiceExtensions
{
    public static void ExServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IBookService, BookService>();
        serviceCollection.AddTransient<ILogService>(provider =>
        {
            bool IsLogEnabled = configuration.GetSection("IsLogEnabled").Get<bool>();
            return new LogService(IsLogEnabled);
        });

    }

}