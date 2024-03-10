using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

        public IEnumerable<IdentityUser> GetUsers()
        {
            var users = userRepository.GetUsers();
            return users;
        }


    }
}