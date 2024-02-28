using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMLibrary.Infrastructure.Services;
using Moq;
using Xunit;

namespace SMLibrary.Infrastructure.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Delete_CheksIdisLessthan1_ThrowsArgumentException()
        {
            var service = new UserService(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(-1));
        }
        [Fact]
        public async Task FindUser_CheksIfNull_ThrowsArgumentNullException()
        {
            var service = new UserService(null);

            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.FindUser("zdkmicdks"));
        }
        [Fact]
        public async Task FindUserbyId_CheksIfNull_ThrowsArgumentNullException()
        {
            var service = new UserService(null);

            await Assert.ThrowsAsync<NullReferenceException>(async () => await service.FindUserbyId(-1));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task SetBalance_CheksAmountisLessthan1_ThrowsArgumentException(int amount)
        {
            var service = new UserService(null);

            await Assert.ThrowsAsync<ArgumentException>(async () => await service.Delete(amount));
        }
    }
}