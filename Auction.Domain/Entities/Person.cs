using Auction.Domain.Entities.Base;
using System.Collections.Generic;

namespace Auction.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }    
        public string Surname { get; set; }
        
        public ICollection<Lot> OwnedLots { get; set; }

        public ICollection<Lot> PurchasedLots { get; set; }
    }
}
