using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class reportmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RS");

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fellowships",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PastorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fellowships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MemberType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisciplerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FellowshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginAccessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPasswordLocked = table.Column<bool>(type: "bit", nullable: true),
                    LoginAttempt = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Fellowships_FellowshipId",
                        column: x => x.FellowshipId,
                        principalSchema: "RS",
                        principalTable: "Fellowships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pastors",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FellowshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginAccessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPasswordLocked = table.Column<bool>(type: "bit", nullable: true),
                    LoginAttempt = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pastors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pastors_Fellowships_FellowshipId",
                        column: x => x.FellowshipId,
                        principalSchema: "RS",
                        principalTable: "Fellowships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityReports",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityReports_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "RS",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityReports_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "RS",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceReports",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFirstTimer = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceReports_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "RS",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceReports_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "RS",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUpReports",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowUpType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalFollowUps = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUpReports_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "RS",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUpReports_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "RS",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutreachReports",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPeopleReached = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutreachReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutreachReports_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "RS",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutreachReports_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "RS",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUpDetails",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowUpReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUpDetails_FollowUpReports_FollowUpReportId",
                        column: x => x.FollowUpReportId,
                        principalSchema: "RS",
                        principalTable: "FollowUpReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutreachDetails",
                schema: "RS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OutreachReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutreachDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutreachDetails_OutreachReports_OutreachReportId",
                        column: x => x.OutreachReportId,
                        principalSchema: "RS",
                        principalTable: "OutreachReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityReports_ActivityId",
                schema: "RS",
                table: "ActivityReports",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityReports_MemberId",
                schema: "RS",
                table: "ActivityReports",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceReports_ActivityId",
                schema: "RS",
                table: "AttendanceReports",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceReports_MemberId",
                schema: "RS",
                table: "AttendanceReports",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpDetails_FollowUpReportId",
                schema: "RS",
                table: "FollowUpDetails",
                column: "FollowUpReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpReports_ActivityId",
                schema: "RS",
                table: "FollowUpReports",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpReports_MemberId",
                schema: "RS",
                table: "FollowUpReports",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_FellowshipId",
                schema: "RS",
                table: "Members",
                column: "FellowshipId");

            migrationBuilder.CreateIndex(
                name: "IX_OutreachDetails_OutreachReportId",
                schema: "RS",
                table: "OutreachDetails",
                column: "OutreachReportId");

            migrationBuilder.CreateIndex(
                name: "IX_OutreachReports_ActivityId",
                schema: "RS",
                table: "OutreachReports",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_OutreachReports_MemberId",
                schema: "RS",
                table: "OutreachReports",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Pastors_FellowshipId",
                schema: "RS",
                table: "Pastors",
                column: "FellowshipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityReports",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "AttendanceReports",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "FollowUpDetails",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "OutreachDetails",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "Pastors",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "FollowUpReports",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "OutreachReports",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "Activities",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "RS");

            migrationBuilder.DropTable(
                name: "Fellowships",
                schema: "RS");
        }
    }
}
