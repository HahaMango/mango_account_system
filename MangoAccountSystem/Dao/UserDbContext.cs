using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangoAccountSystem.Dao
{
	/// <summary>
	/// 用户数据库上下文
	/// </summary>
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

            modelBuilder.Entity<ExternalLoginEntity>()
                .HasIndex(ex => new { ex.LoginProvider, ex.ProviderKey })
                .IsUnique();
        }

        public DbSet<UserEntity> MangoUsers { get; set; }
        public DbSet<UserRoleEntity> MangoUserRoles { get; set; }
        public DbSet<UserClaimEntity> MangoUserClaims { get; set; }
        public DbSet<User2Role> User2Roles { get; set; }
        public DbSet<ExternalLoginEntity> ExternalLogins { get; set; }
    }

	/// <summary>
	/// 用户表
	/// </summary>
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string LoginName { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Column(TypeName = "varchar(40)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(40)")]
        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }

	/// <summary>
	/// 角色表
	/// </summary>
    public class UserRoleEntity
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string RoleName { get; set; }
    }

	/// <summary>
	/// 角色声明表
	/// </summary>
    public class UserClaimEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string ClaimType { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string ClaimValue { get; set; }
    }

	/// <summary>
	/// 角色 用户表
	/// </summary>
    public class User2Role
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

	/// <summary>
	/// 外部登陆表
	/// </summary>
    public class ExternalLoginEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
