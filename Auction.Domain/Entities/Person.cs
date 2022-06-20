using Auction.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace Auction.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }    
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        
        public ICollection<Lot> OwnedLots { get; set; }
        public ICollection<Bid> Bids { get; set; }
    }
}
