using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;

namespace Auction.Data.Repositories
{
    public class BiddingDetailsRepository : BaseRepository<BiddingDetails>, IBiddingDetailsRepository
    {
        public BiddingDetailsRepository(AuctionContext context) : base(context) { }

    }
}
