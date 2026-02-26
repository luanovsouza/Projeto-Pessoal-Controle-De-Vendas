using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleVendasAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConsertandoModeloSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SalesPrice",
                table: "Sales");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Sales",
                type: "TEXT",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalesPrice",
                table: "Sales",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
