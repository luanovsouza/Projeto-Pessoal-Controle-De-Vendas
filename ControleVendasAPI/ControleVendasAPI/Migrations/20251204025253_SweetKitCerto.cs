using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleVendasAPI.Migrations
{
    /// <inheritdoc />
    public partial class SweetKitCerto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SweetKits_Sales_SaleId",
                table: "SweetKits");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "SweetKits",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_SweetKits_Sales_SaleId",
                table: "SweetKits",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SweetKits_Sales_SaleId",
                table: "SweetKits");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "SweetKits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SweetKits_Sales_SaleId",
                table: "SweetKits",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
