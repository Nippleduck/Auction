using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class BidRepository : BaseRepository<Bid>, IBidRepository
    {
        public BidRepository(AuctionContext context) : base(context) { }

        public override async Task<Bid> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Bids
                .Include(bid => bid.Bidder)
                .Include(bid => bid.BiddingDetails)
                .ThenInclude(bd => bd.Lot)
                .FirstOrDefaultAsync(bid => bid.Id == id, ct);

        public override async Task<IEnumerable<Bid>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Bids
                .Include(bid => bid.Bidder)
                .Include(bid => bid.BiddingDetails)
                .ThenInclude(bd => bd.Lot)
                .ToListAsync(ct);
    }
}
