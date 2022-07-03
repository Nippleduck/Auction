using System;

namespace Auction.BusinessModels.Models
{
    public class BidModel
    {
        public int Id { get; set; } 
        public decimal Price { get; set; }
        public DateTime PlacedOn { get; set; }
        public int BidderId { get; set; }
        public string Bidder { get; set; }
    }
}
