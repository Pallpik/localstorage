using HomeLocalStorage.Core.Models;

namespace HomeLocalStorage.Core.DTO
{
    public record CreateFolderDTO
    (
        string Name,
        string Path,
        AccessCodes Status
    );


    /*public record FolderResponseDTO
    (
        Guid Id,
        string Name,
        string Author,
        string Path,
        AccessCodes Status,
        ICollection<FolderResponseDTO> ChildFolders,
        ICollection<MaterialResponse> Materials
    );*/

    public record UpdateFolderDTO(
        Guid Id, string NewName, string Path, AccessCodes Status
        );
}
