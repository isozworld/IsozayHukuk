using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class BureauSafeFix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBureauSafe",
                table: "HuSafes");

            migrationBuilder.AddColumn<int>(
                name: "SafeType",
                table: "HuSafes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SafeType",
                table: "HuSafes");

            migrationBuilder.AddColumn<bool>(
                name: "isBureauSafe",
                table: "HuSafes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
