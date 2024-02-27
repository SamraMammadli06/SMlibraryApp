using System.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;
using SMlibraryApp.Infrastructure.Data;
namespace SMlibraryApp.Infrastructure.Repository;

public class BooksRepository : IBookRepository
{
    private readonly MyDbContext dbContext;

    public BooksRepository(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Create(Book newbook)
    {
        await this.dbContext.Books.AddAsync(newbook);
        await this.dbContext.SaveChangesAsync();
    }

    public async Task DeleteBook(int id)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);

        this.dbContext.Remove<Book>(book);

        await this.dbContext.SaveChangesAsync();
    }

    public async Task<Book?> GetBookById(int id)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);
        return book;
    }
    public async Task BuyBook(int id,string UserName)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);
        var userBook = await dbContext.UserBalances
          .FirstOrDefaultAsync(b => b.UserName == UserName);
        userBook.Balance -= (double)book.Price;
        await dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Book>> GetBooks()
    {
        var books = this.dbContext.Books.AsEnumerable<Book>();
        return books;
    }
    
}

