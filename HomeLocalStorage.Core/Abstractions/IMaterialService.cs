using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core.Models;

namespace HomeLocalStorage.Core.Abstractions
{
    public interface IMaterialService
    {
        Task<(Guid Id, string error)> CreateMaterial(CreateMaterialDTO mDTO, Guid userId);
        Task<(Guid Id, string error)> DeleteMaterial(Guid Id);
        Task<(Material material, string error)> GetMaterialInfo(Guid Id, Guid userId);
        Task<(Guid Id, string error)> UpdateMaterialInfo(UpdateMaterialInfoDTO mDTO, Guid userId);
        Task<(Guid Id, string error)> UpdateMaterialDirectory(RedirectMaterialDTO mDTO, Guid userId);
        // Task<(Guid Id, string error)> UpdateMaterialCore(RewriteMaterialDTO mDTO);
    }
}