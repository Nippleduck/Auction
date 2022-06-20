using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Data.Migrations
{
    public partial class changeBuyerToLotRelationViaDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Persons_BuyerId",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_BuyerId",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Lots");

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "BiddingDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BiddingDetails_BuyerId",
                table: "BiddingDetails",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BiddingDetails_Persons_BuyerId",
                table: "BiddingDetails",
                column: "BuyerId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BiddingDetails_Persons_BuyerId",
                table: "BiddingDetails");

            migrationBuilder.DropIndex(
                name: "IX_BiddingDetails_BuyerId",
                table: "BiddingDetails");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "BiddingDetails");

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Lots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Lots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_BuyerId",
                table: "Lots",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Persons_BuyerId",
                table: "Lots",
                column: "BuyerId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
