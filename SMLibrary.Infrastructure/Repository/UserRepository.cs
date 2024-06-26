using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Data;
using SMLibraryApp.Core.Models;

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

    public async Task<IdentityUser?> FindUser(string UserName)
    {
        var newUser = await this.dbContext.Users.FirstOrDefaultAsync(u => UserName== u.UserName);
        return newUser;
    }

    public IEnumerable<IdentityUser> GetUsers()
    {
        return this.dbContext.Users.AsEnumerable();
    }
    public async Task AddBookToUser(int id, string UserName)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);
       
        if (book != null)
        {
            var bookUserName = new BookUserName
            {
                BookId = id,
                UserName = UserName
            };

            await dbContext.BookUserNames.AddAsync(bookUserName);

            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Book not found", nameof(id));
        }
    }


    public async Task<IEnumerable<Book>> GetBooksbyUser(string UserName)
    {
        var userBook = await dbContext.BookUserNames
          .Where(b => b.UserName == UserName).Select(i=>i.BookId)
          .ToListAsync();
        var books = dbContext.Books.Where(b => userBook.Contains(b.Id));
        return books;
    }

    public async Task Delete(int id){
        var user = await dbContext.Users.FirstOrDefaultAsync(u=>u.Id==id.ToString());
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }
    public async Task<IdentityUser> FindUserbyId(int id){
        var user = await dbContext.Users.FirstOrDefaultAsync(u=>u.Id==id.ToString());
        return user;
    }
}
