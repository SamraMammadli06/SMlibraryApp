using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMLibrary.Core.Models;
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
        var newUser = await this.dbContext.Users.FirstOrDefaultAsync(u => UserName == u.UserName);
        return newUser;
    }

    public async Task DeleteBookbyUser(string UserName, int Id)
    {
        var book = await this.dbContext.BookUserNames.FirstOrDefaultAsync(book => book.BookId == Id);
        var check = dbContext.BookUserNames.Any(ub => ub.UserName == UserName && ub.BookId == Id);
        System.Console.WriteLine("hhh: " + check);
        if (book != null)
        {
            if (check)
            {
                dbContext.BookUserNames.Remove(book);

                await dbContext.SaveChangesAsync();
            }
        }
    }

    public IEnumerable<IdentityUser> GetUsers()
    {
        return this.dbContext.Users.AsEnumerable();
    }
    public async Task<bool> AddBookToUser(int id, string UserName)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);
        var check = dbContext.BookUserNames.Any(ub => ub.UserName == UserName && ub.BookId == id);
        if (book != null)
        {
            if (!check)
            {
                var bookUserName = new BookUserName
                {
                    BookId = id,
                    UserName = UserName,
                };

                await dbContext.BookUserNames.AddAsync(bookUserName);

                await dbContext.SaveChangesAsync();
                return check;
            }
            return check;
        }
        else
        {
            throw new ArgumentException("Book not found", nameof(id));
        }
    }

    public async Task<IEnumerable<Book>> GetMyBooks(string UserName)
    {
        var books = dbContext.Books.Where(book => book.Author == UserName).AsEnumerable<Book>();
        return books;
    }

    public async Task<IEnumerable<Comment>> GetMyComments(string UserName)
    {
        var comments = dbContext.Comments.Where(book => book.Book.Author == UserName).AsEnumerable<Comment>();
        return comments;
    }
    public async Task<IEnumerable<Book>> GetBooksbyUser(string UserName)
    {
        var userBook = await dbContext.BookUserNames
          .Where(b => b.UserName == UserName).Select(i => i.BookId)
          .ToListAsync();
        var books = dbContext.Books.Where(b => userBook.Contains(b.Id));
        return books;
    }

    public async Task<UserCustomUser> GetUser(string UserName)
    {
        var user = await dbContext.UserCustomUsers.FirstOrDefaultAsync(u => u.UserName == UserName);
        return user;
    }

    public async Task Edit(UserCustomUser customUser, string name)
    {
        var user = await dbContext.UserCustomUsers.FirstOrDefaultAsync(u => u.UserName == name);
        if (user is not null)
        {
            user.BannerColor = customUser.BannerColor;
            if (string.IsNullOrWhiteSpace(customUser.ImageUrl) && !string.IsNullOrWhiteSpace(user.ImageUrl))
            {
                user.ImageUrl = user.ImageUrl;
            }
            else
            {
                user.ImageUrl = customUser.ImageUrl;
            }
            user.Description = customUser.Description;
            await dbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentNullException();
        }
    }

    public async Task<UserCustomUser> GetCustomUser(string UserName)
    {
        var user = await dbContext.UserCustomUsers.FirstOrDefaultAsync(u => u.UserName == UserName);
        return user;
    }

    public async Task Delete(string name)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == name);
        var books = dbContext.Books.Where(u => u.Author == name);
        var customUser = await dbContext.UserCustomUsers.FirstOrDefaultAsync(u => u.UserName == name);
        var bookUser = dbContext.BookUserNames.Where(u => u.UserName == name);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            if (books is not null)
            {
                foreach (var book in books)
                {
                    dbContext.Books.Remove(book);
                }
            }
            if (customUser is not null)
            {
                dbContext.UserCustomUsers.Remove(customUser);
            }
            if (bookUser is not null)
            {
                foreach (var book in bookUser)
                {
                    dbContext.BookUserNames.Remove(book);
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task CreateCustomUser(UserCustomUser customUser)
    {
        await this.dbContext.UserCustomUsers.AddAsync(customUser);
        await dbContext.SaveChangesAsync();
    }
    public async Task<IdentityUser> FindUserbyId(int id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
        return user;
    }
}
