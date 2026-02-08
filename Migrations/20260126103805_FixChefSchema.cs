using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeChefss.Migrations
{
    /// <inheritdoc />
    public partial class FixChefSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "HygieneCertificateUrl",
                table: "Chefs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KitchenImageUrl",
                table: "Chefs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HygieneCertificateUrl",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "KitchenImageUrl",
                table: "Chefs");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
        }
    }
}
