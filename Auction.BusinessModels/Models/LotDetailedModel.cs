using System;
using System.Collections.Generic;

namespace Auction.BusinessModels.Models
{
    public class LotDetailedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Seller { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal MinimalBid { get; set; }
        public bool Sold { get; set; }
        public string Buyer { get; set; }
        public IEnumerable<BidModel> Bids { get; set; }
        public string ReviewStatus { get; set; }
        public string Feedback { get; set; }
    }
}
