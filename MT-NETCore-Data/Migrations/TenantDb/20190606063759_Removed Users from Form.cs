using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class RemovedUsersfromForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Forms_FormId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FormId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FormId",
                table: "Users",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Forms_FormId",
                table: "Users",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
