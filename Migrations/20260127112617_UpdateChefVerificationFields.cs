using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeChefss.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChefVerificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminRemarks",
                table: "Chefs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CulinaryCertificateUrl",
                table: "Chefs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GovernmentIdExpiryDate",
                table: "Chefs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovernmentIdImageUrl",
                table: "Chefs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovernmentIdNumber",
                table: "Chefs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationStatus",
                table: "Chefs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Chefs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRemarks",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "CulinaryCertificateUrl",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "GovernmentIdExpiryDate",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "GovernmentIdImageUrl",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "GovernmentIdNumber",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "VerificationStatus",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Chefs");
        }
    }
}
