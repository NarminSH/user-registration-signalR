using System;
using Microsoft.EntityFrameworkCore;
using RegisterMVC.Models;

namespace RegisterMVC.DataAccess
{
    public class UserDbContext: DbContext
    {
        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Role> roles { get; set; }
        public virtual DbSet<RoleUser> RoleUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
            base.OnModelCreating(modelBuilder);
        }

    }
}

