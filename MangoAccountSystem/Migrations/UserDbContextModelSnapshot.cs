﻿// <auto-generated />
using System;
using MangoAccountSystem.Dao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MangoAccountSystem.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MangoAccountSystem.Dao.User2Role", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("User2Roles");
                });

            modelBuilder.Entity("MangoAccountSystem.Dao.UserClaimEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("MangoUserClaims");
                });

            modelBuilder.Entity("MangoAccountSystem.Dao.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Email");

                    b.Property<DateTime>("LastLoginDate");

                    b.Property<string>("LoginName");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("MangoUsers");
                });

            modelBuilder.Entity("MangoAccountSystem.Dao.UserRoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleName");

                    b.HasKey("Id");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("MangoUserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
