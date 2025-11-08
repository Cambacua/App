using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddFechasToDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "AppDestinations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoriaId",
                table: "AppDestinations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
