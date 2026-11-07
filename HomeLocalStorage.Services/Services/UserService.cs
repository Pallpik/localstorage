using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core.Models;
using HomeLocalStorage.Core.Abstractions;

namespace HomeLocalStorage.Services.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<(Guid Id, string error)> CreateUser(CreateUserDTO uDTO)
        {
            var (user, error) = User.Create(Guid.NewGuid(), uDTO.Login, uDTO.Password, uDTO.Login, "user");
            if (string.IsNullOrEmpty(error))
            {
                return (Guid.Empty, error);
            }

            if (!uDTO.Password.Equals(uDTO.SecPassword))
            {
                return (Guid.Empty, "Passwords are not equal!");
            }

            return await _userRepository.CreateUser(user.Id, user.Login, user.Password, user.Name, user.Role);

        }

        public async Task<(Guid Id, string error)> UpdateUserInfo(UpdateUserInfoDTO uDTO, Guid userId)
        {
            var (isValid, error) = User.ValidateName(uDTO.Name);
            if (!isValid)
            {
                return (Guid.Empty, error);
            }

            var role = uDTO.Role == "admin" ? "admin" : "user";

            return await _userRepository.UpdateUserInfoById(userId, uDTO.Name, role);
        }


        public async Task<(Guid id, string error)> UpdateUserData(UpdateUserDataDTO uDTO, Guid userId)
        {
            var (isValid, error) = User.ValidateLogin(uDTO.Login);
            if (!isValid)
            {
                return (Guid.Empty, error);
            }

            (isValid, error) = User.ValidatePassword(uDTO.Password);
            if (!isValid)
            {
                return (Guid.Empty, error);
            }

            if (!uDTO.Password.Equals(uDTO.SecPassword))
            {
                return (Guid.Empty, "Passwords are not equal!");
            }

            return await _userRepository.UpdateUserDataById(userId, uDTO.Login, uDTO.Password);
        }

        public async Task<(Guid Id, string error)> DeleteUser(Guid Id)
        {   
            return await _userRepository.DeleteById(Id);
        }

        public async Task<(User user, string error)> GetUserData(Guid UserId)
        {
            return await _userRepository.GetUserData(UserId);
        }

        public async Task<(Guid Id, string error)> UserLogin(string Login, string Password)
        {
            return await _userRepository.UserLogin(Login, Password);
        }

    }
}
