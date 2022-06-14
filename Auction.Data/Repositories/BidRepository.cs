using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;

namespace Auction.Data.Repositories
{
    public class BidRepository : BaseRepository<Bid>, IBidRepository
    {
        public BidRepository(AuctionContext context) : base(context) { }

    }
}
