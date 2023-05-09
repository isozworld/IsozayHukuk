using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class clienttranficheno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FicheNo",
                table: "HuClientTrans",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FicheNo",
                table: "HuClientTrans");
        }
    }
}
