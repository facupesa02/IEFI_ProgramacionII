using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IEFI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposEspacio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacidad",
                table: "Espacios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Espacios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Espacios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Tamano",
                table: "Espacios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Espacios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacidad",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Tamano",
                table: "Espacios");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Espacios");
        }
    }
}
