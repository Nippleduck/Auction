using Auction.Domain.Entities.Base;

namespace Auction.Domain.Entities
{
    public class LotPlacementRequest : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Reviewed { get; set; }

        public int SellerId { get; set; }
        public Person Seller { get; set; }

        public int ReviewerId { get; set; }
        public Person Reviewer { get; set; }
    }
}
