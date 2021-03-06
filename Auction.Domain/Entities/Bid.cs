using Auction.Domain.Entities.Base;
using System;

namespace Auction.Domain.Entities
{
    public class Bid : BaseEntity
    {
        public decimal Price { get; set; }
        public DateTime PlacedOn { get; set; }

        public int BidderId { get; set; }
        public Person Bidder { get; set; }

        public int BiddingDetailsId { get; set; }
        public BiddingDetails BiddingDetails { get; set; }    
    }
}
