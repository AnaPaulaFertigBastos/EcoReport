using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoReport.Migrations
{
    /// <inheritdoc />
    public partial class InsertTipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TipoDeArea",
                columns: new[] { "Id", "Classificacao" },
                values: new object[,]
                {
                    { 1, "Alagamento" },
                    { 2, "Buraco" },
                    { 3, "Bueiro Entupido" },
                    { 4, "Erosão nas Encostas" },
                    { 5, "Lixo" }

                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
