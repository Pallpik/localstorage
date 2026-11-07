using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core;
using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Repositories;
using HomeLocalStorage.Core.Abstractions;

namespace HomeLocalStorage.Services.Services
{
    public class FolderService(IFolderRepository folderRepository) : IFolderService
    {
        private readonly IFolderRepository _folderRepository = folderRepository;

        /*
          Task<(Guid id, string error)> CreateFolder(string name, string author, string path, AccessStatusCode status, Guid parentFolderId, Guid userId);
        Task<Guid> Delete(Guid FolderId);
        Task<List<Folder>> GetFolderTree();
        Task<List<Folder>> GetFolderTreeByUserId(Guid userId);
        Task<(Guid id, string error)> UpdateFolder(Guid FolderId, string Name, string Path, AccessStatusCode Status);*/

        public async Task<List<Folder>> GetFolders(Guid UserId)
        {
            return await _folderRepository.GetAllFolders(UserId);
        }

        public async Task<List<Folder>> GetFolderByUserId(Guid UserId)
        {
            return await _folderRepository.GetFolders(UserId);
        }

        public async Task<Folder> GetFolder(Guid folderId, Guid userId)
        {
            return await _folderRepository.GetFolder(folderId, userId);
        }

        public async Task<(Guid id, string error)> CreateFolder(CreateFolderDTO f, Guid userId)
        {
            var error = Folder.ValidateName(f.Name);
            if (!string.IsNullOrEmpty(error)) {
                return (Guid.Empty, "");
            }
            return await _folderRepository.CreateFolder(Guid.NewGuid(), f.Name, f.Path, f.Status, userId);

        }

        public async Task<(Guid id, string error)> Delete(Guid FolderId)
        {
            return await _folderRepository.Delete(FolderId);
        }

        public async Task<(Guid id, string error)> UpdateFolder(UpdateFolderDTO f, Guid userId)
        {
            var error = Folder.ValidateName(f.NewName);
            if (!string.IsNullOrEmpty(error))
            {
                return (Guid.Empty, "");
            }

            return await _folderRepository.UpdateFolder(f.Id, f.NewName, f.Path, f.Status, userId);
        }

    }
}
