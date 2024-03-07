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
        
    }
}