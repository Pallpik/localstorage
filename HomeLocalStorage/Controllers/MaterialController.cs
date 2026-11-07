using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeLocalStorage.Controllers
{
    [ApiController]
    [Route("materials")]
    [Authorize]
    public class MaterialController(IWebHostEnvironment environment, IMaterialService materService, IFileManager fileManager) : ControllerBase
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly string BaseStoragePath = "Storage";

        private readonly IMaterialService _materialService = materService;
        private readonly IFileManager _fileManager = fileManager;

        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<Material>> GetMaterialData(Guid Id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            
            var (_m, error) = await _materialService.GetMaterialInfo(Id, userId);
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
                return NotFound(new Material());
            }

            return Ok(_m);

        }

        [HttpPost("new")]
        [RequestSizeLimit(300_000_000)]
        public async Task<ActionResult<SharedDTO>> CreateMaterial([FromForm] CreateMaterialDTO _mDTO)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (id, error) = await _materialService.CreateMaterial(_mDTO, userId);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new SharedDTO(id, error));
            }

            var pathCombined = Path.Combine(_environment.ContentRootPath, BaseStoragePath, _mDTO.Path.TrimStart('/'));


            var ferror = await _fileManager.CreateFile(_mDTO.File, pathCombined, _mDTO.Name, _mDTO.Type);

            if (!string.IsNullOrEmpty(ferror))
            {
                await _materialService.DeleteMaterial(id);
                return StatusCode(500, new SharedDTO(id, ferror));
            }

            return Ok(new SharedDTO(id, ""));
        }



        [HttpPut("update")]
        public async Task<ActionResult<SharedDTO>> UpdateMaterialInfo([FromBody] UpdateMaterialInfoDTO mDTO)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (_m, error) = await _materialService.GetMaterialInfo(mDTO.Id, userId);

            if (!string.IsNullOrEmpty(error))
            {
                return NotFound(new SharedDTO(_m.Id, error));
            }
            if(mDTO.Name != mDTO.OldName) { 
                var ferror = await _fileManager.RenameFile(Path.Combine(_environment.ContentRootPath, BaseStoragePath, mDTO.Path), mDTO.OldName, mDTO.Name, mDTO.Type);
                if (!string.IsNullOrEmpty(ferror))
                {
                    return StatusCode(500, new SharedDTO(Guid.Empty, ferror));
                }
            }
            Guid id;
            (id, error) = await _materialService.UpdateMaterialInfo(mDTO, userId);


            return Ok(new SharedDTO(id, error));

        }

        /*[HttpPut("rewrite")]
        public async Task<ActionResult<>> UpdateMaterialName([FromBody] RewriteMaterialDTO mDTO)
        {
            var (_m, error) = await _materialService.GetMaterialInfo(mDTO.Id);

            if (!string.IsNullOrEmpty(error))
            {
                return NotFound(new SharedDTO(_m.Id, error));
            }

            var ferror = await _fileManager.RenameFile(mDTO.Path, mDTO.OldName, mDTO.Name);
            if (!string.IsNullOrEmpty(ferror))
            {
                return StatusCode(500, new SharedDTO(Guid.Empty, ferror));
            }

            await _materialService.Upda(mDTO);

            return Ok(new SharedDTO(_m.Id, error));
        }*/


        [HttpGet("download/{id:guid}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIdStr == null)
                {
                    return Unauthorized();
                }
                var userId = Guid.Parse(userIdStr);
                var (fileInfo, error) = await _materialService.GetMaterialInfo(id, userId);
                if (!string.IsNullOrEmpty(error))
                {
                    return NotFound("File not found");
                }

                var fileStream = await _fileManager.GetFileStream(
                    Path.Combine(_environment.ContentRootPath, BaseStoragePath, fileInfo.Path),
                    fileInfo.Name,
                    fileInfo.Type);

                if (fileStream == null)
                {
                    return NotFound("File not found on disk");
                }

                string fullFileName = fileInfo.Name + fileInfo.Type; 

                if (!fullFileName.Contains('.') && !fileInfo.Type.StartsWith('.'))
                {
                    fullFileName = fileInfo.Name + "." + fileInfo.Type;
                }

                return File(fileStream, BetterGetContentType(fileInfo.Type), fullFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        private static string BetterGetContentType(string filePath)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


        [HttpPut("redirect")]
        public async Task<ActionResult<SharedDTO>> UpdateMaterialDirectory([FromBody] RedirectMaterialDTO mDTO)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (id, error) = await _materialService.UpdateMaterialDirectory(mDTO, userId);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(new SharedDTO(id, error));
            }

            var ferror = await _fileManager.RedirectFile(Path.Combine(_environment.ContentRootPath, BaseStoragePath, mDTO.Path), mDTO.Name, Path.Combine(_environment.ContentRootPath, BaseStoragePath, mDTO.NewPath) , mDTO.Type);

            if (!string.IsNullOrEmpty(ferror))
            {
                await _materialService.UpdateMaterialDirectory(new RedirectMaterialDTO(mDTO.Id, mDTO.NewPath, mDTO.Name, mDTO.Type, mDTO.Path, mDTO.NewFolderId, mDTO.FolderId), userId);
                return Ok(new SharedDTO(id, ferror));
            }

            return Ok(new SharedDTO(id, error));
        }

        [HttpDelete("delete/{Id:guid}")]
        public async Task<ActionResult<SharedDTO>> DeleteMaterial(Guid Id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(userIdStr);
            var (_m, error) = await _materialService.GetMaterialInfo(Id, userId);
            if (!string.IsNullOrEmpty(error))
            {
                return NotFound(new SharedDTO(_m.Id, error));
            }
            else
            {
                var ferror = await _fileManager.DeleteFile(Path.Combine(_environment.ContentRootPath, BaseStoragePath, _m.Path), _m.Name, _m.Type);

                if (!string.IsNullOrEmpty(ferror))
                {
                    return StatusCode(500, new SharedDTO(Guid.Empty, ferror));
                }

                await _materialService.DeleteMaterial(_m.Id);
                return Ok(new SharedDTO(_m.Id, ""));
            }
        }

    }
}
