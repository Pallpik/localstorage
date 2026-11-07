using HomeLocalStorage.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Configurations
{
    public class FolderConfiguration : IEntityTypeConfiguration<FolderEntity>
    {
        public void Configure(EntityTypeBuilder<FolderEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Status);
            builder.Property(x => x.Path);

            builder.HasOne(x => x.User).WithMany(x => x.Folders).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
