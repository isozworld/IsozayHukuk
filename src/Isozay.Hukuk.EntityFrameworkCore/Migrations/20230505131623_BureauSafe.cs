using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class BureauSafe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBureauSafe",
                table: "HuSafes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBureauSafe",
                table: "HuSafes");
        }
    }
}
