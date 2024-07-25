using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoGH_OnTap.Migrations
{
    public partial class hi5555 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "GHCTs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GHCTs");
        }
    }
}
