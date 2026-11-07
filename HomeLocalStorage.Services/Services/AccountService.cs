using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.Services.Services
{
    public class AccountService(IUserRepository _userRepository, JwtService jwtService)
    {
        public async Task<(Guid id, string error)> Register(string Login, string Name, string Password, string SecPassword)
        {
            var (user, error) = User.Create(Guid.NewGuid(), Login, Password, Name, "user");
            if (!string.IsNullOrEmpty(error))
            {
                return (Guid.Empty, error);
            }

            if (!Password.Equals(SecPassword))
            {
                return (Guid.Empty, "Passwords are not equal!");
            }

            var passHash = new PasswordHasher<User>().HashPassword(user, Password); 
            user.Password = passHash;

            return await _userRepository.CreateUser(user.Id, user.Login, user.Password, user.Name, user.Role);
        }

        public async Task<string> Login(string Login, string Password)
        {
            var (user,  error) = await _userRepository.GetByUserLogin(Login);
            if (!string.IsNullOrEmpty(error))
            {
                return "";
            }
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, Password);
            if (result == PasswordVerificationResult.Success)
            {
               return jwtService.GenerateToken(user);
            }
            else
            {
                return ""; 
            }
        }

    }
}
