using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Data;

namespace SMlibraryApp.Infrastructure.Repository;
public class UserRepository : IUserRepository
{
    private readonly MyDbContext dbContext;
    private readonly UserManager<IdentityUser> userManager;

    public UserRepository(MyDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
    }

    public async Task Create(IdentityUser newuser)
    {
         await this.dbContext.Users.AddAsync(newuser);
    }

    public async Task<IdentityUser?> FindUser(User user)
    {
        var newUser = await this.dbContext.Users.FirstOrDefaultAsync(u => user.Id.ToString() == u.Id);
        return newUser;
    }

    public IEnumerable<IdentityUser> GetUsers()
    {
        return this.dbContext.Users.AsEnumerable();
    }
    
}
