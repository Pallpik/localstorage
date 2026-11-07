using HomeLocalStorage.DataAccess.Entities;
using HomeLocalStorage.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Login).HasMaxLength(User.MAX_LOGIN_LENGTH).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(User.MAX_USERNAME_LENGTH).IsRequired();
        }
    }
}
