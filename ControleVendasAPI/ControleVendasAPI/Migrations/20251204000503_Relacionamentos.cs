using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleVendasAPI.Migrations
{
    /// <inheritdoc />
    public partial class Relacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "SweetKits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Sales",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.CreateIndex(
                name: "IX_SweetKits_SaleId",
                table: "SweetKits",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SweetKits_Sales_SaleId",
                table: "SweetKits",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SweetKits_Sales_SaleId",
                table: "SweetKits");

            migrationBuilder.DropIndex(
                name: "IX_SweetKits_SaleId",
                table: "SweetKits");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "SweetKits");

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Sales",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
