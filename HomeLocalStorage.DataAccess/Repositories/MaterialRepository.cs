using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using HomeLocalStorage.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Repositories
{
    public class MaterialRepository(HomeLocalStorageDbContext context) : IMaterialRepository
    {
        private readonly HomeLocalStorageDbContext _context = context;

        public async Task<(Guid id, string error)> CreateMaterial(Guid id, string name, string description, string type, AccessCodes Code, Guid userId, Guid folderId)
        {
            var mEntity = new MaterialEntity { Id = id, Name = name, Description = description, Type = type, Status = Code, UserId = userId, FolderId = folderId };
            try
            {
                await _context.Materials.AddAsync(mEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                return (Guid.Empty, ex.Message);
            }
            return (mEntity.Id, "");
        }

        public async Task<(Guid id, string error)> DeleteMaterial(Guid id)
        {
            try
            {
                var mEntity = await _context.Materials.FirstOrDefaultAsync(x => x.Id == id);
                if (mEntity == null)
                {
                    return (Guid.Empty, $"There is no material with {id} id");
                }
                _context.Materials.Remove(mEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (Guid.Empty, ex.Message);
            }
            return (id, "");
        }

        public async Task<(Material material, string error)> GetMaterialInfo(Guid id, Guid userId)
        {
            try
            {
                var mEntity = await _context.Materials.Include(x => x.User).Include(x => x.Folder).FirstOrDefaultAsync(x => x.Id == id);
                if (mEntity == null)
                {
                    return (new Material(), $"There is no material with {id} id");
                }
                Console.WriteLine(mEntity.Status + " status");
                var material = new Material(
                    mEntity.Id,
                    mEntity.Name,
                    mEntity.Description,
                    Path.Combine(mEntity.Folder.Path, mEntity.Folder.Name),
                    mEntity.Type,
                    mEntity.User.Name,
                    mEntity.UserId == userId ? mEntity.Status + 2 : mEntity.Status
                    );
                Console.WriteLine(material.Code + " code");
                return (material, "");
            }
            catch (Exception ex)
            {
                return (new Material(), ex.Message);
            }
        }

        public async Task<(Guid id, string error)> UpdateMaterialDirectory(Guid id, Guid newFolderId, Guid userId)
        {
            try
            {
                var mEntity = await _context.Materials.FirstOrDefaultAsync(x => x.Id == id);
                if (mEntity == null)
                {
                    return (Guid.Empty, $"There is no material with {id} id");
                }
                var fEntity = await _context.Folders.FirstOrDefaultAsync(x => x.Id == newFolderId);
                if (fEntity == null)
                {
                    return (Guid.Empty, $"There is no folder with {newFolderId} id");
                }
                if (mEntity.UserId != userId || fEntity.UserId != userId)
                {
                    return (Guid.Empty, "No permission for this!");
                }
                mEntity.FolderId = newFolderId;
                await _context.SaveChangesAsync();
                return (id, "");
            }
            catch (Exception ex)
            {
                return (Guid.Empty, ex.Message);
            }
        }

        public async Task<(Guid id, string error)> UpdateMaterialInfo(Guid id, string name, string description, AccessCodes code, Guid userId)
        {
            try
            {
                var mEntity = await _context.Materials.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
                if (mEntity == null)
                {
                    return (Guid.Empty, $"There is no material with {id} id");
                }
                if (mEntity.UserId != userId) {
                    return (Guid.Empty, "No permission for this!");
                }
                mEntity.Name = name;
                mEntity.Description = description;
                mEntity.Status = code;
                await _context.SaveChangesAsync();
                return (id, "");
            }
            catch (Exception ex)
            {
                return (Guid.Empty, ex.Message);
            }
        }
    }
}

