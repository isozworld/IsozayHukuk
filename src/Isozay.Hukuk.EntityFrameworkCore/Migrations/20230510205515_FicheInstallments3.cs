using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class FicheInstallments3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HuFicheInstallments_FicheId",
                table: "HuFicheInstallments",
                column: "FicheId");

            migrationBuilder.AddForeignKey(
                name: "FK_HuFicheInstallments_HuFiches_FicheId",
                table: "HuFicheInstallments",
                column: "FicheId",
                principalTable: "HuFiches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HuFicheInstallments_HuFiches_FicheId",
                table: "HuFicheInstallments");

            migrationBuilder.DropIndex(
                name: "IX_HuFicheInstallments_FicheId",
                table: "HuFicheInstallments");
        }
    }
}
