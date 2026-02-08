using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeChefss.Migrations
{
    /// <inheritdoc />
    public partial class PickedUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedAt",
                table: "Deliveries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadyAt",
                table: "Deliveries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedAt",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReadyAt",
                table: "Deliveries");
        }
    }
}
