using Auction.Domain.Entities.Base;
using System.Collections.Generic;

namespace Auction.Domain.Entities
{
    public class AuctionStatus : BaseEntity
    {
        public string Name { get; set; }    

        public ICollection<Lot> Lots { get; set; }
    }
}
