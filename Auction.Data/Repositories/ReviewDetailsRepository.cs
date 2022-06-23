using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class ReviewDetailsRepository : BaseRepository<ReviewDetails>, IReviewDetailsRepository
    {
        public ReviewDetailsRepository(AuctionContext context) : base(context) { }

        public override async Task<IEnumerable<ReviewDetails>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Reviews
                .AsNoTracking()
                .Include(r => r.Lot)
                .ToListAsync(ct);

        public override async Task<ReviewDetails> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Reviews
                .Include(r => r.Lot)
                .FirstOrDefaultAsync(r => r.Id == id, ct);

        public async Task<ReviewDetails> GetByLotIdAsync(int lotId, CancellationToken ct = default) =>
            await context.Reviews.FirstOrDefaultAsync(r => r.LotId == lotId, ct);   
    }
}
