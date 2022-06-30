using System;

namespace Auction.BusinessModels.Models
{
    public class SaleLotModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
    }
}
