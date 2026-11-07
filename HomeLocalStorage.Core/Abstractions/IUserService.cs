using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core.Models;

namespace HomeLocalStorage.Core.Abstractions
{
    public interface IUserService
    {
        Task<(Guid Id, string error)> CreateUser(CreateUserDTO uDTO);
        Task<(Guid Id, string error)> DeleteUser(Guid Id);
        Task<(Guid Id, string error)> UserLogin(string Login, string Password);
        Task<(User user, string error)> GetUserData(Guid Id);
        Task<(Guid id, string error)> UpdateUserData(UpdateUserDataDTO uDTO, Guid userId);
        Task<(Guid Id, string error)> UpdateUserInfo(UpdateUserInfoDTO uDTO, Guid userId);
    }
}