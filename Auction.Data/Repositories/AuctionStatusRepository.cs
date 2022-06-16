using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class AuctionStatusRepository : BaseRepository<AuctionStatus>, IAuctionStatusRepository
    {
        public AuctionStatusRepository(AuctionContext context) : base(context) { }

        public override async Task<IEnumerable<AuctionStatus>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Statuses
                .AsNoTracking()
                .Include(s => s.Lots)
                .ToListAsync(ct);

        public override async Task<AuctionStatus> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Statuses
                .Include(s => s.Lots)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
    }
}
