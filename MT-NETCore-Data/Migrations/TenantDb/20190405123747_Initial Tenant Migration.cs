using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class InitialTenantMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Team_Ref = table.Column<string>(nullable: true),
                    OfflineId = table.Column<string>(nullable: true),
                    MaxUsers = table.Column<long>(nullable: false),
                    MaxRecord = table.Column<long>(nullable: false),
                    MaxRecordsPerMth = table.Column<long>(nullable: false),
                    RecordsThisMth = table.Column<long>(nullable: false),
                    RecordsThisYear = table.Column<long>(nullable: false),
                    MaxForms = table.Column<long>(nullable: false),
                    Industry = table.Column<string>(nullable: true),
                    TeamSize = table.Column<string>(nullable: true),
                    DatePaid = table.Column<DateTime>(nullable: false),
                    ResetDate = table.Column<DateTime>(nullable: false),
                    NextSubscriptionDate = table.Column<DateTime>(nullable: false),
                    LastApiTimestamp = table.Column<DateTime>(nullable: false),
                    PaymentLastApiTimestamp = table.Column<DateTime>(nullable: false),
                    SubscriptionID = table.Column<string>(nullable: true),
                    CustomerSubscriptionID = table.Column<string>(nullable: true),
                    DisplayCampaignTab = table.Column<bool>(nullable: false),
                    DisplayReportTab = table.Column<bool>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    LogoLink = table.Column<string>(nullable: true),
                    CustomerAquisition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TeamId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    FormJson = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true),
                    NumberOFSubmissions = table.Column<long>(nullable: false),
                    NumberOFApprovedSubmissions = table.Column<long>(nullable: false),
                    NumberOFUnApprovedSubmissions = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    FormId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FormId = table.Column<byte[]>(nullable: true),
                    SubmissionJson = table.Column<string>(nullable: true),
                    SubmissionPosition = table.Column<long>(nullable: false),
                    LocationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    BVNNumber = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    PrimaryLocationId = table.Column<string>(nullable: true),
                    SecondaryLocationId = table.Column<string>(nullable: true),
                    IdString = table.Column<string>(nullable: true),
                    PhotoString = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    FormId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true),
                    TeamId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Locations_PrimaryLocationId",
                        column: x => x.PrimaryLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Locations_SecondaryLocationId",
                        column: x => x.SecondaryLocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProjectId",
                table: "Forms",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_FormId",
                table: "Locations",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TeamId",
                table: "Projects",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_LocationId",
                table: "Submissions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FormId",
                table: "Users",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrimaryLocationId",
                table: "Users",
                column: "PrimaryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectId",
                table: "Users",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SecondaryLocationId",
                table: "Users",
                column: "SecondaryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
