using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Models.Migrations
{
    public partial class migrationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserGroupRoles",
                table: "IdentityUserGroupRoles");

            migrationBuilder.RenameTable(
                name: "IdentityUserGroupRoles",
                newName: "case_usergroup_role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_case_usergroup_role",
                table: "case_usergroup_role",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_case_usergroup_role",
                table: "case_usergroup_role");

            migrationBuilder.RenameTable(
                name: "case_usergroup_role",
                newName: "IdentityUserGroupRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserGroupRoles",
                table: "IdentityUserGroupRoles",
                column: "Id");
        }
    }
}
