using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;

namespace SMLibrary.Infrastructure.Services;
public class BookServices : IBookServices
{
    private readonly IBookRepository bookRepository;

    public BookServices(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;

    }
    public async Task<IEnumerable<Book>> GetBooks()
    {
        var books = await bookRepository.GetBooks();
        if(books is null || !books.Any()){
            return Enumerable.Empty<Book>();
        }
        return books;
    }
    public async Task<Book?> GetBookById(int id){
        if(id<1){
            throw new ArgumentException("id can't be less than zero");
        }
        var book = await bookRepository.GetBookById(id);
        if (book is null)
        {
            throw new ArgumentNullException($"Book with id {id} not found");
        }
        return book;
    }
    public async Task Create(Book newbook){
        if(string.IsNullOrWhiteSpace(newbook.Name) || string.IsNullOrWhiteSpace(newbook.Content)){
            throw new ArgumentException("Can't be empty");
        }
        await bookRepository.Create(newbook);
    }
    public async Task DeleteBook(int id){
        if(id<1){
            throw new ArgumentException("id can't be less than zero");
        }
        await bookRepository.DeleteBook(id);
    }
    public async Task ChangeBook(int id, Book newbook){

    }
}
