using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoReport.Migrations
{
    /// <inheritdoc />
    public partial class FixPonto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PontoTipoDeArea_TipoDeArea_TipoDeAreaId",
                table: "PontoTipoDeArea");

            migrationBuilder.AlterColumn<int>(
                name: "TipoDeAreaId",
                table: "PontoTipoDeArea",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Ponto",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_PontoTipoDeArea_TipoDeArea_TipoDeAreaId",
                table: "PontoTipoDeArea",
                column: "TipoDeAreaId",
                principalTable: "TipoDeArea",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PontoTipoDeArea_TipoDeArea_TipoDeAreaId",
                table: "PontoTipoDeArea");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Ponto");

            migrationBuilder.AlterColumn<int>(
                name: "TipoDeAreaId",
                table: "PontoTipoDeArea",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PontoTipoDeArea_TipoDeArea_TipoDeAreaId",
                table: "PontoTipoDeArea",
                column: "TipoDeAreaId",
                principalTable: "TipoDeArea",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
