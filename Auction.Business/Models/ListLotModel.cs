using System;

namespace Auction.Business.Models
{
    public class ListLotModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
        public int BidsCount { get; set; }
    }
}
