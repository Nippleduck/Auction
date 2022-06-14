using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;

namespace Auction.Data.Repositories
{
    public class AuctionStatusRepository : BaseRepository<AuctionStatus>, IAuctionStatusRepository
    {
        public AuctionStatusRepository(AuctionContext context) : base(context) { }

    }
}
