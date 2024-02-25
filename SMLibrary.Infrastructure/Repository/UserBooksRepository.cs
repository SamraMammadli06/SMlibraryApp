using Microsoft.EntityFrameworkCore;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Infrastructure.Data;
using SMLibraryApp.Core.Models;
using SMLibraryApp.Core.Repository;

namespace SMLibrary.Infrastructure.Repository;

public class UserBooksRepository : IUserBooksRepository
{
    private readonly MyDbContext dbContext;

    public UserBooksRepository(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
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
}
