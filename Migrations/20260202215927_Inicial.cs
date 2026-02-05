using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoPyme.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clase",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Tamano",
                table: "Productos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Clase",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tamano",
                table: "Productos",
                type: "int",
                nullable: true);
        }
    }
}
