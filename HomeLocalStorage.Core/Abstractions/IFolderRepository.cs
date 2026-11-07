using HomeLocalStorage.Core.Models;

namespace HomeLocalStorage.Core.Abstractions
{
    public interface IFolderRepository
    {
        Task<(Guid id, string error)> CreateFolder(Guid Id, string name, string path, AccessCodes code, Guid userId);
        Task<(Guid id, string error)> Delete(Guid folderId);
        Task<List<Folder>> GetAllFolders(Guid UserId);
        Task<Folder> GetFolder(Guid folderId, Guid userId);
        Task<List<Folder>> GetFolders(Guid userId);
        Task<(Guid id, string error)> UpdateFolder(Guid folderId, string name, string path, AccessCodes code, Guid userId);
    }
}