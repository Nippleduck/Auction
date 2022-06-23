using System;

namespace Auction.ApiModels.Reviews.Requests
{
    public class ApprovePlacementRequest
    {
        public int LotId { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Feedback { get; set; }
    }
}
