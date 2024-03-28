using System.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SMLibrary.Core.Models;
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

    public async Task ChangeBook(Book book)
    {
        var oldBook = await this.dbContext.Books.FirstOrDefaultAsync(b => b.Name == book.Name && b.Author==book.Author);

        if (oldBook != null)
        {
            oldBook.Name = book.Name;
            oldBook.Author = book.Author;
            oldBook.Image = book.Image;
            oldBook.Description = book.Description;
            oldBook.Content = book.Content;
            oldBook.IsFinished = book.IsFinished;
            oldBook.tag = book.tag;

            await this.dbContext.SaveChangesAsync();
        }
    }

    public async Task<Book?> GetBookById(int id)
    {
        var book = await this.dbContext.Books.FirstOrDefaultAsync(book => book.Id == id);
        return book;
    }

    public async Task<IEnumerable<Book>> GetBooks()
    {
        var books = this.dbContext.Books.AsEnumerable<Book>();
        return books;
    }

    public async Task<IEnumerable<Book>> GetBooksByTag(string tag)
    {
        if (Enum.TryParse<Tag>(tag, out var tagEnum))
        {
            return await dbContext.Books.Where(book => book.tag == tagEnum).ToListAsync();
        }
        else
        {
            return Enumerable.Empty<Book>();
        }
    }



}

