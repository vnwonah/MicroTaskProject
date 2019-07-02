using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class UpdatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Forms",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MaxNumberOfAgents",
                table: "Forms",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "PricePerResponse",
                table: "Forms",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfAgents",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "PricePerResponse",
                table: "Forms");
        }
    }
}
