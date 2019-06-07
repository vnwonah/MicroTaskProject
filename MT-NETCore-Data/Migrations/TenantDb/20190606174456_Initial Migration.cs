using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MT_NetCore_Data.Migrations.TenantDb
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UTCCreatedAt = table.Column<DateTime>(nullable: false),
                    UTCModifiedAt = table.Column<DateTime>(nullable: true),
                    UTCDeletedAt = table.Column<DateTime>(nullable: true),
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UTCCreatedAt = table.Column<DateTime>(nullable: false),
                    UTCModifiedAt = table.Column<DateTime>(nullable: true),
                    UTCDeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UTCCreatedAt = table.Column<DateTime>(nullable: false),
                    UTCModifiedAt = table.Column<DateTime>(nullable: true),
                    UTCDeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    FormJson = table.Column<string>(nullable: true),
                    ProjectId = table.Column<long>(nullable: false),
                    SubmissionCount = table.Column<long>(nullable: false),
                    DeletedCount = table.Column<long>(nullable: false),
                    ApprovedCount = table.Column<long>(nullable: false),
                    RejectedCount = table.Column<long>(nullable: false),
                    InvalidatedCount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UTCCreatedAt = table.Column<DateTime>(nullable: false),
                    UTCModifiedAt = table.Column<DateTime>(nullable: true),
                    UTCDeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    FormId = table.Column<long>(nullable: true)
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
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UTCCreatedAt = table.Column<DateTime>(nullable: false),
                    UTCModifiedAt = table.Column<DateTime>(nullable: true),
                    UTCDeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FormId = table.Column<long>(nullable: false),
                    RecordJson = table.Column<string>(nullable: true),
                    LocationId = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UTCCreatedAt = table.Column<DateTime>(nullable: false),
                    UTCModifiedAt = table.Column<DateTime>(nullable: true),
                    UTCDeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PrimaryLocationId = table.Column<long>(nullable: true),
                    SecondaryLocationId = table.Column<long>(nullable: true),
                    TeamId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Locations_PrimaryLocationId",
                        column: x => x.PrimaryLocationId,
                        principalTable: "Locations",
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    FormId = table.Column<long>(nullable: false),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormUsers", x => new { x.FormId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FormUsers_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    ProjectId = table.Column<long>(nullable: false),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ProjectId",
                table: "Forms",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FormUsers_UserId",
                table: "FormUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_FormId",
                table: "Locations",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TeamId",
                table: "Projects",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_LocationId",
                table: "Records",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrimaryLocationId",
                table: "Users",
                column: "PrimaryLocationId");

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
                name: "FormUsers");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "Records");

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
