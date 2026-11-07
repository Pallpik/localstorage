using HomeLocalStorage.Core.Models;

namespace HomeLocalStorage.Core.Abstractions
{
    public interface IUserRepository
    {
        Task<(Guid id, string error)> UpdateUserDataById(Guid UserId, string Login, string Password);
        Task<(Guid id, string error)> CreateUser(Guid Id, string Login, string Password, string Name, string Role = "User");
        Task<(Guid id, string error)> DeleteById(Guid UserId);
        Task<(Guid id, string error)> UserLogin(string Login, string Password);
        Task<(User user, string error)> GetUserData(Guid Id);
        Task<(User user, string error)> GetByUserLogin(string Login);
        Task<(Guid id, string error)> UpdateUserInfoById(Guid UserId, string Name, string Role);
    }
}