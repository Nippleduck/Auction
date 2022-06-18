using Auction.Domain.Entities.Base;

namespace Auction.Domain.Entities
{
    public class LotImage : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] FullSize { get; set; }
        public byte[] Thumbnail { get; set; }

        public int LotId { get; set; }
        public Lot Lot { get; set; }
    }
}
