using System;

namespace Auction.BusinessModels.Models
{
    public class UserLotSaleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public bool Sold { get; set; }
        public string ReviewStatus { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
        public BidModel HighestBid { get; set; }
    }
}
