using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;

namespace SMLibrary.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository repository;
        public BookService(IBookRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<Book>> GetBooks()
        {
            var books = await repository.GetBooks();
            if (books is null)
            {
                return Enumerable.Empty<Book>();
            }
            return books;
        }

        public async Task<IEnumerable<Book>> GetBooksByTag(string tag)
        {
            var books = await repository.GetBooksByTag(tag);
            if (books is null)
            {
                return Enumerable.Empty<Book>();
            }
            return books;
        }
        public async Task ChangeBook(Book book)
        {
            await repository.ChangeBook(book);
        }
        public async Task<Book?> GetBookById(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Can't be less than 1");
            }
            var book = await repository.GetBookById(id);
            if (book is null)
            {
                throw new ArgumentException("Not found");
            }
            return book;
        }
        public async Task Create(Book newbook)
        {
            await repository.Create(newbook);
        }
        public async Task DeleteBook(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Can't be less than 1");
            }
            await repository.DeleteBook(id);
        }

    }
}