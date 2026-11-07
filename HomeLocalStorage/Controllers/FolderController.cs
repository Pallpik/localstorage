using HomeLocalStorage.Core;
using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeLocalStorage.Controllers
{
    [ApiController]
    [Route("folders")]
    [Authorize]
    public class FolderController(IWebHostEnvironment environment, IFolderService folderService, IFolderManager folderManager) : ControllerBase
    {
        private readonly IFolderService _folderService = folderService;
        private readonly IFolderManager _folderManager = folderManager;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly string BaseStoragePath = "Storage";

        [HttpGet("get")]
        public async Task<ActionResult<List<Folder>>> GetFolders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) {
                return Unauthorized();
            }
            return Ok(await _folderService.GetFolders(Guid.Parse(userId)));
        }

        [HttpGet("{folderId:guid}")]
        public async Task<ActionResult<Folder>> GetFolderById(Guid folderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            return Ok(await _folderService.GetFolder(folderId, Guid.Parse(userId)));
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<Folder>>> GetFoldersByUserUId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            return Ok(await _folderService.GetFolderByUserId(Guid.Parse(userId)));
        }

        [HttpPost("create")]
        public async Task<ActionResult<SharedDTO>> CreateFolder([FromBody] CreateFolderDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var (id, error) = await _folderService.CreateFolder(request, Guid.Parse(userId));
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new SharedDTO(id, error));
            
            }
            var ferror = await _folderManager.CreateFolder(Path.Combine(_environment.ContentRootPath, BaseStoragePath, request.Path), request.Name);
            if (!string.IsNullOrEmpty(ferror)) {
                await _folderService.Delete(id);
                return StatusCode(500, new SharedDTO(Guid.Empty, ferror));
            }
            
            return Ok(new SharedDTO(id, error));
        }

        [HttpPut("update")]
        public async Task<ActionResult<SharedDTO>> UpdateFolder([FromBody]UpdateFolderDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var f = await _folderService.GetFolder(request.Id, Guid.Parse(userId));

            if (f.Id != request.Id)
            {
                return NotFound(new SharedDTO(f.Id, "Not Found"));
            }
            if (!(f.Name == request.NewName))
            {
                var ferror = await _folderManager.UpdateFolderName(Path.Combine(_environment.ContentRootPath, BaseStoragePath, request.Path), f.Name, request.NewName);
                if (!string.IsNullOrEmpty(ferror))
                {
                    return StatusCode(500, new SharedDTO(Guid.Empty, ferror));
                }
            }
            var (id, error) = await _folderService.UpdateFolder(request, Guid.Parse(userId));

            return Ok(new SharedDTO(id, error));
        }

        [HttpDelete("delete/{folderId:guid}")]
        public async Task<ActionResult<SharedDTO>> DeleteFolder(Guid folderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var f = await _folderService.GetFolder(folderId, Guid.Parse(userId));
            if (f.Id != folderId)
            {
                return NotFound(new SharedDTO(f.Id, "Not Found"));
            }
            else
            {
                var ferror = await _folderManager.DeleteFolder(Path.Combine(_environment.ContentRootPath, BaseStoragePath, f.Path), f.Name);

                if (!string.IsNullOrEmpty(ferror))
                {
                    return StatusCode(500, new SharedDTO(Guid.Empty, ferror));
                }

                await _folderService.Delete(f.Id);
                return Ok(new SharedDTO(f.Id, ""));
            }
        }

    }
}
