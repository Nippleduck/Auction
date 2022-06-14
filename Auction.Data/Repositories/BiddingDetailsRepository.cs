using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class BiddingDetailsRepository : BaseRepository<BiddingDetails>, IBiddingDetailsRepository
    {
        public BiddingDetailsRepository(AuctionContext context) : base(context) { }

        public async Task<BiddingDetails> GetByIdWithDetailsAsync(int id, CancellationToken ct) =>
            await context.BiddingDetails
                .Include(bd => bd.Bids)
                .ThenInclude(bid => bid.Bidder)
                .FirstOrDefaultAsync(bid => bid.Id == id, ct);

        public async Task<IEnumerable<BiddingDetails>> GetAllWithDetailsAsync(CancellationToken ct) =>
            await context.BiddingDetails
                .AsNoTracking()
                .Include(bd => bd.Bids)
                .ThenInclude(bid => bid.Bidder)
                .ToListAsync(ct);
    }
}
