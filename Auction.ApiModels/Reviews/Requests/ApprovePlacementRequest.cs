using System;

namespace Auction.ApiModels.Reviews.Requests
{
    public class ApprovePlacementRequest
    {
        public int LotId { get; set; }
        public int StatusId { get; set; }
        public int MinimalBid { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Feedback { get; set; }
    }
}
