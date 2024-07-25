using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoGH_OnTap.Migrations
{
    public partial class hi555566666 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GHCTs_GioHang_GioHangId",
                table: "GHCTs");

            migrationBuilder.DropForeignKey(
                name: "FK_GHCTs_SanPhams_SanPhamId",
                table: "GHCTs");

            migrationBuilder.AlterColumn<Guid>(
                name: "SanPhamId",
                table: "GHCTs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "GioHangId",
                table: "GHCTs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_GHCTs_GioHang_GioHangId",
                table: "GHCTs",
                column: "GioHangId",
                principalTable: "GioHang",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GHCTs_SanPhams_SanPhamId",
                table: "GHCTs",
                column: "SanPhamId",
                principalTable: "SanPhams",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GHCTs_GioHang_GioHangId",
                table: "GHCTs");

            migrationBuilder.DropForeignKey(
                name: "FK_GHCTs_SanPhams_SanPhamId",
                table: "GHCTs");

            migrationBuilder.AlterColumn<Guid>(
                name: "SanPhamId",
                table: "GHCTs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GioHangId",
                table: "GHCTs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GHCTs_GioHang_GioHangId",
                table: "GHCTs",
                column: "GioHangId",
                principalTable: "GioHang",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GHCTs_SanPhams_SanPhamId",
                table: "GHCTs",
                column: "SanPhamId",
                principalTable: "SanPhams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
