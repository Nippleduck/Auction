using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Data.Migrations
{
    public partial class addMinimalBidProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinimalBid",
                table: "BiddingDetails",
                type: "decimal(10, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimalBid",
                table: "BiddingDetails");
        }
    }
}
