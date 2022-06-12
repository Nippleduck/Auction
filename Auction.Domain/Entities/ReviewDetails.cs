using Auction.Domain.Entities.Base;
using Auction.Domain.Entities.Enums;

namespace Auction.Domain.Entities
{
    public class ReviewDetails : BaseEntity
    {
        public ReviewStatus Status { get; set; }
        public string Feedback { get; set; }

        public int LotId { get; set; }
        public Lot Lot { get; set; }
    }
}
