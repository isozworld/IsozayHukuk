using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class ficheandclientupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxNumber",
                table: "HuClients",
                newName: "Mail");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HuFicheLines",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ClientIdentifier",
                table: "HuClients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "HuClients",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "HuFicheLines");

            migrationBuilder.DropColumn(
                name: "ClientIdentifier",
                table: "HuClients");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "HuClients");

            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "HuClients",
                newName: "TaxNumber");
        }
    }
}
