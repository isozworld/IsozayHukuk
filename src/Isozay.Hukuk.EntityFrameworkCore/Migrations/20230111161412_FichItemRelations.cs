using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class FichItemRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HuFicheLines_ItemId",
                table: "HuFicheLines",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_HuFicheLines_HuItems_ItemId",
                table: "HuFicheLines",
                column: "ItemId",
                principalTable: "HuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HuFicheLines_HuItems_ItemId",
                table: "HuFicheLines");

            migrationBuilder.DropIndex(
                name: "IX_HuFicheLines_ItemId",
                table: "HuFicheLines");
        }
    }
}
