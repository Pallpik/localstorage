using HomeLocalStorage.Core.Models;

namespace HomeLocalStorage.Core.Abstractions
{
    public interface IMaterialRepository
    {
        Task<(Guid id, string error)> CreateMaterial(Guid id, string name, string description, string type, AccessCodes Code, Guid userId, Guid folderId);
        Task<(Guid id, string error)> DeleteMaterial(Guid id);
        Task<(Material material, string error)> GetMaterialInfo(Guid id, Guid userId);
        Task<(Guid id, string error)> UpdateMaterialInfo(Guid id, string name, string description, AccessCodes code, Guid userId);
        Task<(Guid id, string error)> UpdateMaterialDirectory(Guid id, Guid newFolderId, Guid userId);
        //  Task<(Guid id, string error)> UpdateMaterialCore(Guid Id, string Name, string Type, string Description, AccessStatusCode Code);
    }
}