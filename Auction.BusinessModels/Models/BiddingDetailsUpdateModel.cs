using System;

namespace Auction.BusinessModels.Models
{
    public class BiddingDetailsUpdateModel
    {
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public int MinimalBid { get; set; }
    }
}
