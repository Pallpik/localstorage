using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using HomeLocalStorage.Core.Abstractions;

namespace HomeLocalStorage.DataAccess.Repositories
{
    public class FolderRepository(HomeLocalStorageDbContext context) : IFolderRepository
    {
        private readonly HomeLocalStorageDbContext _context = context;

        public async Task<(Guid id, string error)> CreateFolder(Guid Id, string name, string path, AccessCodes code, Guid userId)
        {
            var fEntity = new FolderEntity
            {
                Id = Id,
                Name = name,
                Path = path,
                Status = code,
                UserId = userId
            };
            try
            {
                await _context.Folders.AddAsync(fEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (Guid.Empty, ex.Message);
            }

            return (Id, "");
        }

        public async Task<(Guid id, string error)> Delete(Guid folderId)
        {
            try
            {
                var fEntity = await _context.Folders.FirstOrDefaultAsync(x => x.Id == folderId);
                if (fEntity == null)
                {
                    return (Guid.Empty, $"There is no folder with {folderId} id");
                }
                _context.Folders.Remove(fEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (Guid.Empty, ex.Message);
            }
            return (folderId, "");
        }

        public async Task<List<Folder>> GetAllFolders(Guid UserId)
        {
            var fEntities = await _context.Folders.Include(x => x.Materials).Include(x => x.User).ToListAsync();
            
            var folders = new List<Folder>();
            foreach (var e in fEntities)
            {
                if (e == null) continue;
                if (e.Status == AccessCodes.REALNONE && !(e.UserId == UserId)) continue;

                var folder = new Folder(e.Id, e.Name, e.User.Name, e.UserId == UserId ? e.Status + 2 : e.Status)
                {
                    Path = e.Path
                };
                folder.Materials = [.. e.Materials.Select(m =>
                new Material(
                    m.Id,
                    m.Name,
                    m.Description,
                    Path.Combine(folder.Path, folder.Name),
                    m.Type,
                    m.User.Name,
                    m.UserId == UserId ? m.Status + 2 : m.Status))];
                folders.Add(folder);
            }

            return folders;
        }

        public async Task<Folder> GetFolder(Guid folderId, Guid userId)
        {
            var e = await _context.Folders.Include(x => x.User).Include(x => x.Materials).FirstOrDefaultAsync(x => x.Id == folderId);
            if (e == null)
            {
                return new Folder();
            }
            if (e.UserId != userId && e.Status == AccessCodes.REALNONE)
                return new Folder();
            var folder = new Folder(e.Id, e.Name, e.User.Name, e.UserId == userId ? e.Status + 2 : e.Status)
            {
                Path = e.Path
            };
            folder.Materials = [.. e.Materials.Select(m =>
                new Material(
                    m.Id,
                    m.Name,
                    m.Description,
                    Path.Combine(folder.Path, folder.Name),
                    m.Type,
                    m.User.Name,
                    m.UserId == userId ? m.Status + 2 : m.Status))];
            
            return folder;
        }

        public async Task<List<Folder>> GetFolders(Guid userId)
        {
            var fEntities = await _context.Folders.Include(x => x.User).Include(x => x.Materials).Where(x => x.UserId == userId).ToListAsync();
            var folders = new List<Folder>();
            foreach (var e in fEntities)
            {
                var folder = new Folder(e.Id, e.Name, e.User.Name, e.Status + 2)
                {
                    Path = e.Path
                };
                folder.Materials = [.. e.Materials.Select(m =>
                new Material(
                    m.Id,
                    m.Name,
                    m.Description,
                    Path.Combine(folder.Path, folder.Name),
                    m.Type,
                    m.User.Name,
                    m.Status + 2))];
                folders.Add(folder);
            }

            return folders;
        }

        public async Task<(Guid id, string error)> UpdateFolder(Guid folderId, string name, string path, AccessCodes code, Guid userId)
        {
            var fEntity = await _context.Folders.FirstOrDefaultAsync(x => x.Id == folderId);
            if (fEntity == null)
            {
                return (Guid.Empty, $"There is no folder with {folderId} id");
            }
            if (fEntity.UserId != userId)
            {
                return (Guid.Empty, $"No permission for this!");
            }
            fEntity.Name = name;
            fEntity.Path = path;
            fEntity.Status = code;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (Guid.Empty, ex.Message);
            }

            return (folderId, "");
        }
    }
}
