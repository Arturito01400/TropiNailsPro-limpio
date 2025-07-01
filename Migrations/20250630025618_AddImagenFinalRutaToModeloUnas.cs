using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropiNailsPro.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenFinalRutaToModeloUnas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenFinalRuta",
                table: "ModelosUnas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenFinalRuta",
                table: "ModelosUnas");
        }
    }
}
