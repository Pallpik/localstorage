using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using HomeLocalStorage.Core.Abstractions;

namespace HomeLocalStorage.DataAccess.Repositories
{
    public class UserRepository(HomeLocalStorageDbContext context) : IUserRepository
    {
        private readonly HomeLocalStorageDbContext _context = context;

        public async Task<(Guid id, string error)> UserLogin(string Login, string Password)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Login == Login);
            if (userEntity == null)
            {
                return (Guid.Empty, "There is no user with this login!");
            }
            if (userEntity.Password != Password)
            {
                return (Guid.Empty, "Wrong password!");
            }

            return (userEntity.Id, "");
        }

        public async Task<(User user, string error)> GetByUserLogin(string Login)
        {
            var u = await _context.Users.FirstOrDefaultAsync(x => x.Login == Login);
            if (u == null)
            {
                return (new User(), "Пользователя с таким логином не существует!");
            }
            return User.Create(u.Id, u.Login, u.Password, u.Name, u.Role);
        }

        public async Task<(User user, string error)> GetUserData(Guid Id)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            if (userEntity == null)
            {
                return (new User(), $"There is no user with {Id} id!");
            }

            var (user, error) = User.Create(userEntity.Id, userEntity.Login, userEntity.Password, userEntity.Name, userEntity.Role);
            return (user, error);
        }

        public async Task<(Guid id, string error)> CreateUser(Guid Id, string Login, string Password, string Name, string Role = "user")
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Login == Login);
            if (userEntity != null)
            {
                return (Guid.Empty, "This login is taken!");
            }

            var newUser = new UserEntity
            {
                Id = Id,
                Login = Login,
                Password = Password,
                Name = Name,
                Role = Role
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return (newUser.Id, "");
        }

        public async Task<(Guid id, string error)> UpdateUserDataById(Guid UserId, string Login, string Password)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (userEntity == null)
            {
                return (UserId, $"There is no user with ID {UserId}");
            }

            userEntity.Login = Login;
            userEntity.Password = Password;
            await _context.SaveChangesAsync();
            return (UserId, "");

        }

        public async Task<(Guid id, string error)> UpdateUserInfoById(Guid UserId, string Name, string Role)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (userEntity == null)
            {
                return (UserId, $"There is no user with ID {UserId}");
            }

            userEntity.Role = Role;
            userEntity.Name = Name;
            await _context.SaveChangesAsync();
            return (UserId, "");
        }

        public async Task<(Guid id, string error)> DeleteById(Guid UserId)
        {
            UserEntity? userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (userEntity == null)
            {
                return (UserId, $"There is no user with ID {UserId}");
            }
            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();
            return (UserId, "");
        }
    }
}
