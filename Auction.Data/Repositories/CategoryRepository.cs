using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;


namespace Auction.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AuctionContext context) : base(context) { }

    }
}
