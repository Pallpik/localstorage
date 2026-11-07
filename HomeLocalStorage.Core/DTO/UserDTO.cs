using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.Core.DTO
{
    public record UserResponse
    (
        Guid Id,
        string Name,
        string Login,
        string Role,
        string Error

    );
    public record LoginUserRequest
   (
       string Login,
       string Password
   );
    public record UpdateUserDataDTO(
        string Login,
        string Password,
        string SecPassword
    );


    public record UpdateUserInfoDTO(
        string Name,
        string Role
    );

    public record CreateUserDTO(
       string Login,
       string Password,
       string SecPassword,
       string Name
    );
}
