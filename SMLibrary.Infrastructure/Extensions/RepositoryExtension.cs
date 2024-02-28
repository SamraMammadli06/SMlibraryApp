using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Repository;

namespace SMLibrary.Infrastructure.Extensions;
public static class RepositoryExtensions
{
    public static void ExRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IBookRepository, BooksRepository>();
        serviceCollection.AddScoped<ILogRepository, LogRepository>();
    }
}
