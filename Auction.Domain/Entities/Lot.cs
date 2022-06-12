using Auction.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace Auction.Domain.Entities
{
    public class Lot : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public BiddingDetails BiddingDetails { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public int SellerId { get; set; }
        public Person Seller { get; set; }
    }
}
