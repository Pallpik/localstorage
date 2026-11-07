using HomeLocalStorage.Core.DTO;
using HomeLocalStorage.Core.Models;
using HomeLocalStorage.Core.Abstractions;
using HomeLocalStorage.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace HomeLocalStorage.Services.Services
{
    public class MaterialService(IMaterialRepository repository) : IMaterialService
    {
        private readonly IMaterialRepository _repository = repository;

        public async Task<(Material material, string error)> GetMaterialInfo(Guid Id, Guid userId)
        {
            return await _repository.GetMaterialInfo(Id, userId);
        }

        public async Task<(Guid Id, string error)> CreateMaterial(CreateMaterialDTO m, Guid userId)
        {
            var error = Material.ValidatePath(m.Path).error;

            if (!string.IsNullOrEmpty(error))
            {
                return (Guid.Empty, error);
            }

            error = Material.ValidateName(m.Name).error;
            if (!string.IsNullOrEmpty(error))
            {
                return (Guid.Empty, error);
            }



            return await _repository.CreateMaterial(Guid.NewGuid(), m.Name, m.Description, m.Type, m.Status, userId, m.FolderId);
        }

        public async Task<(Guid Id, string error)> DeleteMaterial(Guid Id)
        {
            return await _repository.DeleteMaterial(Id);
        }

        public async Task<(Guid Id, string error)> UpdateMaterialInfo(UpdateMaterialInfoDTO mDTO, Guid userId)
        {
            var (b, error) = Material.ValidateName(mDTO.Name);
            if (!b)
                return (Guid.Empty, error);

            return await _repository.UpdateMaterialInfo(mDTO.Id, mDTO.Name, mDTO.Description, mDTO.Status, userId);

        }

        public async Task<(Guid Id, string error)> UpdateMaterialDirectory(RedirectMaterialDTO mDTO, Guid userId)
        {
            var (b, error) = Material.ValidateName(mDTO.Name);
            if (!b)
                return (Guid.Empty, error);
            (b, error) = Material.ValidatePath(mDTO.NewPath);
            if (!b)
                return (Guid.Empty, error);

            return await _repository.UpdateMaterialDirectory(mDTO.Id, mDTO.NewFolderId, userId);
        }

        /*public async Task<(Guid Id, string error)> UpdateMaterialCore(RewriteMaterialDTO mDTO)
        {
            var (b, error) = Material.ValidateName(mDTO.Name);
            if (!b)
                return (Guid.Empty, error);
            (b, error) = Material.ValidatePath(mDTO.Path);
            if (!b)
                return (Guid.Empty, error);

            return await _repository.UpdateMaterialCore(mDTO.Id, mDTO.Name, mDTO.Type, mDTO.Description, mDTO.Status);
        }*/
    }
}
