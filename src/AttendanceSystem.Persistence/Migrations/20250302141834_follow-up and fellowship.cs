using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class followupandfellowship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "RS",
                table: "FollowUpDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "DiscipleId",
                schema: "RS",
                table: "FollowUpDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                schema: "RS",
                table: "Fellowships",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscipleId",
                schema: "RS",
                table: "FollowUpDetails");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "RS",
                table: "Fellowships");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "RS",
                table: "FollowUpDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
