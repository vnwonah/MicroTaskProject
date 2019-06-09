using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class UpdatedRecordModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalTitle",
                table: "Records",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalTitle",
                table: "Records");
        }
    }
}
