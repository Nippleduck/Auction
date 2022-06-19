using System;

namespace Auction.Business.Models
{
    public class LotModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
        public decimal MinimalBid { get; set; }
        public int BidsCount { get; set; }
    }
}
