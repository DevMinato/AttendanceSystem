using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class followupupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                schema: "RS",
                table: "FollowUpReports");

            migrationBuilder.DropColumn(
                name: "FollowUpType",
                schema: "RS",
                table: "FollowUpReports");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "RS",
                table: "FollowUpReports");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "RS",
                table: "FollowUpDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FollowUpType",
                schema: "RS",
                table: "FollowUpDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "MemberId",
                schema: "RS",
                table: "FollowUpDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                schema: "RS",
                table: "FollowUpDetails");

            migrationBuilder.DropColumn(
                name: "FollowUpType",
                schema: "RS",
                table: "FollowUpDetails");

            migrationBuilder.DropColumn(
                name: "MemberId",
                schema: "RS",
                table: "FollowUpDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                schema: "RS",
                table: "FollowUpReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FollowUpType",
                schema: "RS",
                table: "FollowUpReports",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "RS",
                table: "FollowUpReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
