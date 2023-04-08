using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Isozay.Hukuk.Migrations
{
    public partial class ClientTran_DebtCreditBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "HuClientTrans",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                table: "HuClientTrans",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Debt",
                table: "HuClientTrans",
                type: "decimal(65,30)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "HuClientTrans");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "HuClientTrans");

            migrationBuilder.DropColumn(
                name: "Debt",
                table: "HuClientTrans");
        }
    }
}
