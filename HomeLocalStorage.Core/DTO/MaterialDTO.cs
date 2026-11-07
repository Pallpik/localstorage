using HomeLocalStorage.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.Core.DTO
{
    public record MaterialResponse
    (
        Guid Id,
        string Name,
        string Type,
        string Path,
        string Description,
        AccessCodes Status,
        string Author,
        string Error
    );

    public record FileResponse(

        string Name,
        string Path,
        string Type
    );

    public record UpdateMaterialInfoDTO(
        Guid Id,
        string Name,
        string Path,
        string Description,
        string OldName,
        string Type,
        AccessCodes Status
    );

    public record RedirectMaterialDTO(
        Guid Id,
        string Path,
        string Name,
        string Type,
        string NewPath,
        Guid FolderId,
        Guid NewFolderId
    );

    public record RewriteMaterialDTO(
        Guid Id,
        string Path,
        string Name,
        IFormFile NewFile
    );

    public record CreateMaterialDTO(

        string Name,
        string Path,
        string Description,
        string Type,
        IFormFile File,
        AccessCodes Status,
        Guid FolderId

    );
}
