using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMLibrary.Infrastructure.Services;
using Xunit;
using Moq;

namespace SMLibrary.Infrastructure.Test.Services
{
    public class BookServicesTests
    {
        [Fact]
        public async Task GetBooks_ChecksIfBooksEmptyorNull_ReturnsEmpty()
        {
            var bookServiceMock = new Mock<IBookServices>();
            bookServiceMock.Setup((service) => service.GetBooks())
                .ReturnsAsync(Enumerable.Empty<Book>());
        }
        [Fact]
        public async Task GetBookById_CheksIdisLessthan1_ThrowsArgumentException()
        {
            var service = new BookServices(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetBookById(-1));
        }
         
        [Fact]
        public async Task DeleteBook_CheksIdisLessthan1_ThrowsArgumentException()
        {
            var service = new BookServices(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteBook(-1));
        }
        [Theory]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task Create_CheckBookNameEmptyorNull_ThrowsArgumentException(string name)
        {
            var service = new BookServices(null);
            var book = new Book()
            {
                Name = name
            };
             await Assert.ThrowsAsync<ArgumentException>(() => service.Create(book));
    
        }
    }
}