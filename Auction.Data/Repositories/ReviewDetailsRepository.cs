using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;

namespace Auction.Data.Repositories
{
    public class ReviewDetailsRepository : BaseRepository<ReviewDetails>, IReviewDetailsRepository
    {
        public ReviewDetailsRepository(AuctionContext context) : base(context) { }

    }
}
