using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class Addedpropertiestouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountNumber",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Users");
        }
    }
}
