using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class BidRepository : BaseRepository<Bid>, IBidRepository
    {
        public BidRepository(AuctionContext context) : base(context) { }

        public override async Task<Bid> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Bids
                .AsNoTracking()
                .Include(bid => bid.Bidder)
                .Include(bid => bid.BiddingDetails)
                .ThenInclude(bd => bd.Lot)
                .FirstOrDefaultAsync(bid => bid.Id == id, ct);

        public override async Task<IEnumerable<Bid>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Bids
                .AsNoTracking()
                .Include(bid => bid.Bidder)
                .Include(bid => bid.BiddingDetails)
                .ThenInclude(bd => bd.Lot)
                .ToListAsync(ct);

        public async Task<Bid> GetHighestBidderAsync(int lotId, CancellationToken ct = default) =>
            await context.Bids
                .AsNoTracking()
                .Include(bid => bid.Bidder)
                .Include(bid => bid.BiddingDetails)
                .Where(bid => bid.BiddingDetails.LotId == lotId)
                .OrderByDescending(bid => bid.Price)
                .FirstOrDefaultAsync(ct);
    }
}
