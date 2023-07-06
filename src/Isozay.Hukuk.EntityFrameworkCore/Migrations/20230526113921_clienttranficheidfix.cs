using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class clienttranficheidfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HuClientTrans_HuFiches_FicheId",
                table: "HuClientTrans");

            migrationBuilder.DropIndex(
                name: "IX_HuClientTrans_FicheId",
                table: "HuClientTrans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HuClientTrans_FicheId",
                table: "HuClientTrans",
                column: "FicheId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HuClientTrans_HuFiches_FicheId",
                table: "HuClientTrans",
                column: "FicheId",
                principalTable: "HuFiches",
                principalColumn: "Id");
        }
    }
}
