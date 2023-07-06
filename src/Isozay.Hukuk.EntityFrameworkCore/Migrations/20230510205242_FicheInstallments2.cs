using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class FicheInstallments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HuFicheInstallments_HuCurrencies_CurrencyId",
                table: "HuFicheInstallments");

            migrationBuilder.DropIndex(
                name: "IX_HuFicheInstallments_CurrencyId",
                table: "HuFicheInstallments");

            migrationBuilder.AlterColumn<long>(
                name: "CurrencyId",
                table: "HuFicheInstallments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CurrencyId",
                table: "HuFicheInstallments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_HuFicheInstallments_CurrencyId",
                table: "HuFicheInstallments",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_HuFicheInstallments_HuCurrencies_CurrencyId",
                table: "HuFicheInstallments",
                column: "CurrencyId",
                principalTable: "HuCurrencies",
                principalColumn: "Id");
        }
    }
}
