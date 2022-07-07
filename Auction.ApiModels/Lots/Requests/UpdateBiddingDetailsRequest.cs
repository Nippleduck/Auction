﻿using System;

namespace Auction.ApiModels.Lots.Requests
{
    public class UpdateBiddingDetailsRequest
    {
        public int LotId { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; } 
        public int MinimalBid { get; set; }
    }
}
