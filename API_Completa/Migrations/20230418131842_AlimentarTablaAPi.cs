using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APICompleta.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaAPi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Apis",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Una nueva y magnifica Api", new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4822), new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4783), "", 50, "Nueva Api", 5, 200.0 },
                    { 2, "", "Una nueva y magnifica Api 2", new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4830), new DateTime(2023, 4, 18, 9, 18, 41, 973, DateTimeKind.Local).AddTicks(4828), "", 50, "Nueva Api 2", 5, 200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Apis",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Apis",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
