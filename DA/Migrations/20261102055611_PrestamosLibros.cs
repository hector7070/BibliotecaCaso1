using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class PrestamosLibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrestamosLibros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamosLibros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrestamosLibros_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrestamosLibros_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrestamosLibros_FechaPrestamo",
                table: "PrestamosLibros",
                column: "FechaPrestamo");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamosLibros_LibroId_Estado",
                table: "PrestamosLibros",
                columns: new[] { "LibroId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_PrestamosLibros_UsuarioId",
                table: "PrestamosLibros",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrestamosLibros");
        }
    }
}
