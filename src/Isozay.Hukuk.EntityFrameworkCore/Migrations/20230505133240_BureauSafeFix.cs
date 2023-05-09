using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class BureauSafeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBureauSafe",
                table: "HuSafes",
                newName: "BureauSafe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BureauSafe",
                table: "HuSafes",
                newName: "IsBureauSafe");
        }
    }
}
