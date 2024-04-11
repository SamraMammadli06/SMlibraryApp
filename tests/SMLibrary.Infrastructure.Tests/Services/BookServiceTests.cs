using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMLibrary.Infrastructure.Services;
using Moq;
using Xunit;

namespace SMLibrary.Infrastructure.Tests.Repository
{
    public class BookServiceTests
    {
        [Fact]
        public async Task GetBooks_ChecksIfBooksEmptyorNull_ReturnsEmpty()
        {
            var bookServiceMock = new Mock<IBookService>();
            bookServiceMock.Setup((service) => service.GetBooks())
                .ReturnsAsync(Enumerable.Empty<Book>());
        }
        [Fact]
        public async Task GetBookById_CheksIdisLessthan1_ThrowsArgumentException()
        {
            var service = new BookService(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetBookById(-1));
        }

        [Fact]
        public async Task DeleteBook_CheksIdisLessthan1_ThrowsArgumentException()
        {
            var service = new BookService(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteBook(-1));
        }
        [Fact]
        public async Task BuyBook_CheksIdisLessthan1_ThrowsArgumentException()
        {
            var service = new BookService(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.BuyBook(-1,"name"));
        }
        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task BuyBook_CheksUserNameEmptyorNull_ThrowsArgumentNullException(string name)
        {
            var service = new BookService(null);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.BuyBook(1,name));
        }
    }
}