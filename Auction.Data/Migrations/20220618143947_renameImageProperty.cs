using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Data.Migrations
{
    public partial class renameImageProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quality",
                table: "Images");

            migrationBuilder.AddColumn<byte[]>(
                name: "FullSize",
                table: "Images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullSize",
                table: "Images");

            migrationBuilder.AddColumn<byte[]>(
                name: "Quality",
                table: "Images",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
