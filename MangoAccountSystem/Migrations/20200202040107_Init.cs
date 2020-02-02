using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MangoAccountSystem.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MangoUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(15)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangoUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MangoUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangoUserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MangoUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LoginName = table.Column<string>(type: "varchar(20)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(20)", nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(type: "varchar(40)", nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LastLoginDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangoUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User2Roles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User2Roles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangoUserRoles_RoleName",
                table: "MangoUserRoles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangoUsers_UserName",
                table: "MangoUsers",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangoUserClaims");

            migrationBuilder.DropTable(
                name: "MangoUserRoles");

            migrationBuilder.DropTable(
                name: "MangoUsers");

            migrationBuilder.DropTable(
                name: "User2Roles");
        }
    }
}
