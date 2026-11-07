using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.DataAccess.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeLocalStorage.Controllers
{

    [ApiController]
    [Route("mypage")]
    [Authorize]
    public class UserController(IUserService userService, IFolderService folderService, IFolderManager folderManager, IWebHostEnvironment environment) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IFolderManager _folderManager = folderManager;
        private readonly IFolderService _folderService = folderService;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly string BaseStoragePath = "Storage";

        [HttpGet("profile")]
        public async Task<ActionResult<UserResponse>> GetUser()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (user, error) = await _userService.GetUserData(userId);
            return Ok(new UserResponse(user.Id, user.Name, user.Login, user.Role, error));
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<SharedDTO>> DeleteUser()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var folders = await _folderService.GetFolderByUserId(userId);
            foreach (var f in folders)
            {
                await _folderManager.DeleteFolder(Path.Combine(_environment.ContentRootPath, BaseStoragePath, f.Path), f.Name);
            }
            var (id, error) = await _userService.DeleteUser(userId);
            
            return Ok(new SharedDTO(id, error));
        }

        [HttpPut("edit/data")]
        public async Task<ActionResult<SharedDTO>> UpdateUserData([FromBody] UpdateUserDataDTO uDTO)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (id, error) = await _userService.UpdateUserData(uDTO, userId);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new SharedDTO(id, error));
            }
            return Ok(new SharedDTO(id, error));
        }

        [HttpPut("edit/info")]
        public async Task<ActionResult<SharedDTO>> UpdateUserInfo([FromBody] UpdateUserInfoDTO uDTO)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (id, error) = await _userService.UpdateUserInfo(uDTO, userId);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new SharedDTO(id, error));
            }
            return Ok(new SharedDTO(id, error));
        }

    }
}
