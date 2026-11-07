using HomeLocalStorage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Entities
{
    public class FolderEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public AccessCodes Status { get; set; }

        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; } = null!;
        public virtual ICollection<MaterialEntity> Materials { get; set; } = [];

    }
}
