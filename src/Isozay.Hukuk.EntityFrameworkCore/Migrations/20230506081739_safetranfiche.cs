using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class safetranfiche : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FicheId",
                table: "HuSafeTrans",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HuSafeTrans_FicheId",
                table: "HuSafeTrans",
                column: "FicheId");

            migrationBuilder.AddForeignKey(
                name: "FK_HuSafeTrans_HuFiches_FicheId",
                table: "HuSafeTrans",
                column: "FicheId",
                principalTable: "HuFiches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HuSafeTrans_HuFiches_FicheId",
                table: "HuSafeTrans");

            migrationBuilder.DropIndex(
                name: "IX_HuSafeTrans_FicheId",
                table: "HuSafeTrans");

            migrationBuilder.DropColumn(
                name: "FicheId",
                table: "HuSafeTrans");
        }
    }
}
