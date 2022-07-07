using System;

namespace Auction.BusinessModels.Models
{
    public class ReviewApprovalModel
    {
        public int LotId { get; set; }
        public int StatusId { get; set; }
        public int MinimalBid { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Feedback { get; set; }
    }
}
