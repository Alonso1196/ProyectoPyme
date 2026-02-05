using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoPyme.Migrations
{
    /// <inheritdoc />
    public partial class AddEsencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EsenciaId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "esencias",
                columns: table => new
                {
                    EsenciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_esencias", x => x.EsenciaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "esencias",
                columns: new[] { "EsenciaId", "Activo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Eau de Toilette" },
                    { 2, true, "Eau de Parfum" },
                    { 3, true, "Parfum" },
                    { 4, true, "Elixir" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_EsenciaId",
                table: "Productos",
                column: "EsenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_esencias_EsenciaId",
                table: "Productos",
                column: "EsenciaId",
                principalTable: "esencias",
                principalColumn: "EsenciaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_esencias_EsenciaId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "esencias");

            migrationBuilder.DropIndex(
                name: "IX_Productos_EsenciaId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "EsenciaId",
                table: "Productos");
        }
    }
}
