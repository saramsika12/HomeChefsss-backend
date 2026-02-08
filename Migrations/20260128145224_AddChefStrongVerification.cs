using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeChefss.Migrations
{
    /// <inheritdoc />
    public partial class AddChefStrongVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveComplaints",
                table: "Chefs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChefLevel",
                table: "Chefs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Chefs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SecurityDepositAmount",
                table: "Chefs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "SecurityDepositPaid",
                table: "Chefs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrialEndsAt",
                table: "Chefs",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveComplaints",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "ChefLevel",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "SecurityDepositAmount",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "SecurityDepositPaid",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "TrialEndsAt",
                table: "Chefs");
        }
    }
}
