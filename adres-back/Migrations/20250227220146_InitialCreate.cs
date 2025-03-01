using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace adres.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adquisicion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Presupuesto = table.Column<decimal>(type: "numeric", nullable: false),
                    Unidad = table.Column<string>(type: "text", nullable: false),
                    TipoBienServicio = table.Column<string>(type: "text", nullable: false),
                    Cantidad = table.Column<long>(type: "bigint", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Proveedor = table.Column<string>(type: "text", nullable: false),
                    Documentacion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adquisicion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "historico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdquisicionId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoCambio = table.Column<int>(type: "integer", nullable: false),
                    DetalleCambio = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_historico_adquisicion_AdquisicionId",
                        column: x => x.AdquisicionId,
                        principalTable: "adquisicion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_historico_AdquisicionId",
                table: "historico",
                column: "AdquisicionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "historico");

            migrationBuilder.DropTable(
                name: "adquisicion");
        }
    }
}
