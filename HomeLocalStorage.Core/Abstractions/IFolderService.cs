using HomeLocalStorage.Core.Models;
using HomeLocalStorage.Core.DTO;

namespace HomeLocalStorage.Core.Abstractions
{
    public interface IFolderService
    {
        Task<(Guid id, string error)> CreateFolder(CreateFolderDTO f, Guid UserId);
        Task<(Guid id, string error)> Delete(Guid FolderId);
        Task<List<Folder>> GetFolderByUserId(Guid UserId);
        Task<List<Folder>> GetFolders(Guid UserId);
        Task<Folder> GetFolder(Guid FolderId, Guid UserId);
        Task<(Guid id, string error)> UpdateFolder(UpdateFolderDTO f, Guid UserId);
    }
}