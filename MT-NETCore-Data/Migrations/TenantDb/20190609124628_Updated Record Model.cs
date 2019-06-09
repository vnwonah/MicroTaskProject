using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class UpdatedRecordModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SentToDevice",
                table: "Records",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentToDevice",
                table: "Records");
        }
    }
}
