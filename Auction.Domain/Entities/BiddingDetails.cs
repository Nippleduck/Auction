using System;
using System.Collections.Generic;

namespace Auction.Domain.Entities
{
    public class BiddingDetails
    {
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
        public bool Sold { get; set; }
        public int BidsCount => Bids.Count;

        public ICollection<Bid> Bids { get; set; }

        public int LotId { get; set; }
        public Lot Lot { get; set; }

        public int HighestBidderId { get; set; }
        public Person HighestBidder { get; set; }
    }
}
