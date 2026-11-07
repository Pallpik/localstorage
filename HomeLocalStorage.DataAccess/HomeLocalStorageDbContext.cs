using HomeLocalStorage.DataAccess.Configurations;
using HomeLocalStorage.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess
{
    public class HomeLocalStorageDbContext(DbContextOptions<HomeLocalStorageDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FolderConfiguration());
            modelBuilder.ApplyConfiguration(new MaterialConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<FolderEntity> Folders { get; set; }
        public DbSet<MaterialEntity> Materials { get; set; }

    }
}
