using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace MangoAccountSystem.Dao
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions dbContextOptionsBuilder) : base(dbContextOptionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasIndex(ue => ue.UserName)
                .IsUnique();

            modelBuilder.Entity<UserRoleEntity>()
                .HasIndex(ure => ure.RoleName)
                .IsUnique();

            modelBuilder.Entity<User2Role>()
                .HasKey(u2r => new { u2r.UserId, u2r.RoleId });
        }

        public DbSet<UserEntity> MangoUsers { get; set; }
        public DbSet<UserRoleEntity> MangoUserRoles { get; set; }
        public DbSet<UserClaimEntity> MangoUserClaims { get; set; }
        public DbSet<User2Role> User2Roles { get; set; }
    }

    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }

    public class UserRoleEntity
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
    }

    public class UserClaimEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class User2Role
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
