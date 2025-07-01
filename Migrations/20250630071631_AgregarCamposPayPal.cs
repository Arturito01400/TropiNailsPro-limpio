using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropiNailsPro.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposPayPal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "PagoTransferencia",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PayPalId",
                table: "PagoTransferencia",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "PagoTransferencia");

            migrationBuilder.DropColumn(
                name: "PayPalId",
                table: "PagoTransferencia");
        }
    }
}
