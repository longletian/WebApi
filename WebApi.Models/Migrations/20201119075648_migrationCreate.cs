using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace WebApi.Models.Migrations
{
    public partial class migrationCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "case_account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    AccountName = table.Column<string>(nullable: false),
                    AccountPasswd = table.Column<string>(nullable: false),
                    AccountPasswdEncrypt = table.Column<string>(nullable: false),
                    AccountType = table.Column<long>(nullable: false),
                    AccountIp = table.Column<string>(nullable: true),
                    AccountState = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_group_user",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: false),
                    UserGroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_group_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_login_account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    AccountName = table.Column<string>(nullable: false),
                    LoginType = table.Column<long>(nullable: false),
                    LoginIp = table.Column<string>(nullable: true),
                    Eimi = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Brower = table.Column<string>(nullable: true),
                    LoginTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_login_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_menu",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    MenuName = table.Column<string>(nullable: false),
                    MenuPath = table.Column<string>(nullable: false),
                    MenuCode = table.Column<string>(nullable: false),
                    ParentMenuCode = table.Column<string>(nullable: true),
                    MenuUrl = table.Column<string>(nullable: false),
                    MenuRemark = table.Column<string>(nullable: true),
                    SortNum = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_permission",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    PermissionCode = table.Column<string>(nullable: false),
                    PermissionName = table.Column<string>(nullable: false),
                    ParentPermissionCode = table.Column<string>(nullable: true),
                    PermissionRemark = table.Column<string>(nullable: true),
                    SortNum = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    RoleCode = table.Column<string>(nullable: false),
                    RoleName = table.Column<string>(nullable: false),
                    ParentCode = table.Column<string>(nullable: true),
                    RoleRemark = table.Column<string>(nullable: true),
                    SortNum = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_role_permission",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PermissionId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_role_permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_user",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    TelePhone = table.Column<string>(nullable: true),
                    UserImageUrl = table.Column<string>(nullable: true),
                    BirthDay = table.Column<string>(nullable: true),
                    UserSex = table.Column<long>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    ProvinceCode = table.Column<string>(nullable: true),
                    CityCode = table.Column<string>(nullable: true),
                    DistrictCode = table.Column<string>(nullable: true),
                    UserAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_user_group",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreateorId = table.Column<long>(nullable: false),
                    UpdateorId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    UserGroupName = table.Column<string>(nullable: false),
                    UserGroupCode = table.Column<string>(nullable: false),
                    ParentUserGroupCode = table.Column<string>(nullable: true),
                    UserGroupRemark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_user_group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "case_user_role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_user_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserGroupRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserGroupId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserGroupRoles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case_account");

            migrationBuilder.DropTable(
                name: "case_group_user");

            migrationBuilder.DropTable(
                name: "case_login_account");

            migrationBuilder.DropTable(
                name: "case_menu");

            migrationBuilder.DropTable(
                name: "case_permission");

            migrationBuilder.DropTable(
                name: "case_role");

            migrationBuilder.DropTable(
                name: "case_role_permission");

            migrationBuilder.DropTable(
                name: "case_user");

            migrationBuilder.DropTable(
                name: "case_user_group");

            migrationBuilder.DropTable(
                name: "case_user_role");

            migrationBuilder.DropTable(
                name: "IdentityUserGroupRoles");
        }
    }
}
