using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeChefss.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToChefAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Orders");

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLocation_Latitude",
                table: "Orders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLocation_Longitude",
                table: "Orders",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "KitchenLocation_Latitude",
                table: "Chefs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "KitchenLocation_Longitude",
                table: "Chefs",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryLocation_Latitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLocation_Longitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "KitchenLocation_Latitude",
                table: "Chefs");

            migrationBuilder.DropColumn(
                name: "KitchenLocation_Longitude",
                table: "Chefs");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
