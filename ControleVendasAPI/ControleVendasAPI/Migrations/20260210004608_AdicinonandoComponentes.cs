using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleVendasAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdicinonandoComponentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Custo",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Faturamento",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Lucro",
                table: "Sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Custo",
                table: "Sales",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Faturamento",
                table: "Sales",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lucro",
                table: "Sales",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
