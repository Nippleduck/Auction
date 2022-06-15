using Auction.Domain.Entities.Base;
using System.Collections.Generic;

namespace Auction.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Lot> Lots { get; set; }
    }
}
