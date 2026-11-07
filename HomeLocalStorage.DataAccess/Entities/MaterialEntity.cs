using HomeLocalStorage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Entities
{
    public class MaterialEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public AccessCodes Status { get; set; }

        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; } = null!;

        public Guid FolderId { get; set; }
        public virtual FolderEntity Folder { get; set; } = null!;
    }
}
