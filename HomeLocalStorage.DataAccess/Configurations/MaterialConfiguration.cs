using HomeLocalStorage.Core.Models;
using HomeLocalStorage.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Configurations
{
    public class MaterialConfiguration : IEntityTypeConfiguration<MaterialEntity>
    {
        public void Configure(EntityTypeBuilder<MaterialEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(Material.MAX_NAME_LENGTH);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Materials).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Folder).WithMany(x => x.Materials).HasForeignKey(x => x.FolderId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}


/*
        public Guid Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public AccessStatusCode Status { get; set; }
 


        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; } = null!;
 */