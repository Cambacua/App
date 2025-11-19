using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBuddy.Migrations
{
    /// <inheritdoc />
    public partial class addDestination4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "AppDestinations");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "AppDestinations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "AppDestinations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "AppDestinations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
