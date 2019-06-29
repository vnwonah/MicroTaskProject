using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class changedIdFieldValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BVNNumber",
                table: "Users",
                newName: "IdentityNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentityNumber",
                table: "Users",
                newName: "BVNNumber");
        }
    }
}
