using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeChefss.Migrations
{
    /// <inheritdoc />
    public partial class AddChefWalletAndDeposit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DepositPaidAt",
                table: "Chefs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Chefs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SecurityDepositBalance",
                table: "Chefs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalEarnings",
                table: "Chefs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WithdrawableBalance",
                table: "Chefs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositPaidAt",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "SecurityDepositBalance",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "TotalEarnings",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "WithdrawableBalance",
                table: "Chefs");
        }
    }
}
