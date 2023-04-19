using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICompleta.Migrations
{
    /// <inheritdoc />
    public partial class agregarNumeroApiTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroApis",
                columns: table => new
                {
                    ApiNo = table.Column<int>(type: "int", nullable: false),
                    ApiId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroApis", x => x.ApiNo);
                    table.ForeignKey(
                        name: "FK_NumeroApis_Apis_ApiId",
                        column: x => x.ApiId,
                        principalTable: "Apis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Apis",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 19, 9, 2, 44, 552, DateTimeKind.Local).AddTicks(6713), new DateTime(2023, 4, 19, 9, 2, 44, 552, DateTimeKind.Local).AddTicks(6678) });

            migrationBuilder.UpdateData(
                table: "Apis",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 19, 9, 2, 44, 552, DateTimeKind.Local).AddTicks(6719), new DateTime(2023, 4, 19, 9, 2, 44, 552, DateTimeKind.Local).AddTicks(6717) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroApis_ApiId",
                table: "NumeroApis",
                column: "ApiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroApis");

            migrationBuilder.UpdateData(
                table: "Apis",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4822), new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4783) });

            migrationBuilder.UpdateData(
                table: "Apis",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4830), new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4828) });
        }
    }
}
