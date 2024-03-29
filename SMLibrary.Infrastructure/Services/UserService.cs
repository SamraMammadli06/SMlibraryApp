using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SMLibrary.Core.Models;
using SMLibrary.Core.Services;
using SMlibraryApp.Core.Models;
using SMlibraryApp.Core.Repository;

namespace SMLibrary.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public async Task<bool> AddBookToUser(int id, string UserName)
        {
            var check = await userRepository.AddBookToUser(id, UserName);
            return check;
        }

        public async Task Create(IdentityUser newuser)
        {
            await userRepository.Create(newuser);
        }

        public async Task Delete(string name)
        {
            await userRepository.Delete(name);
        }


        public async Task<IdentityUser?> FindUser(string UserName)
        {
            var user = await userRepository.FindUser(UserName);
            if (user is null)
            {
                throw new ArgumentNullException("Wrong data");
            }
            return user;
        }

        public async Task<IdentityUser> FindUserbyId(int id)
        {
            var user = await userRepository.FindUserbyId(id);
            if (user is null)
            {
                throw new ArgumentNullException("Wrong data");
            }
            return user;
        }
        public async Task<UserCustomUser> GetCustomUser(string UserName)
        {
            var user = await userRepository.GetCustomUser(UserName);
            return user;
        }
        public async Task Edit(UserCustomUser customUser)
        {
            await userRepository.Edit(customUser);
        }
        public async Task CreateCustomUser(UserCustomUser customUser)
        {
            await userRepository.CreateCustomUser(customUser);
        }
        public async Task<IEnumerable<Book>> GetMyBooks(string UserName)
        {
            var books = await userRepository.GetMyBooks(UserName);

            return books;
        }

        public async Task<IEnumerable<Book>> GetBooksbyUser(string UserName)
        {
            var books = await userRepository.GetBooksbyUser(UserName);

            return books;
        }

        public async Task DeleteBookbyUser(string UserName,int Id)
        {
            await userRepository.DeleteBookbyUser(UserName,Id);
        }

        public IEnumerable<IdentityUser> GetUsers()
        {
            var users = userRepository.GetUsers();
            return users;
        }

        public async Task<UserCustomUser> GetUser(string UserName)
        {
            var user = await userRepository.GetUser(UserName);
            return user;
        }

    }
}