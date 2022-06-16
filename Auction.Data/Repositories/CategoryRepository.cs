using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AuctionContext context) : base(context) { }

        public override async Task<IEnumerable<Category>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Categories
                .AsNoTracking()
                .Include(c => c.Lots)
                .ToListAsync(ct);

        public override async Task<Category> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Categories
                .Include(c => c.Lots)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
    }
}
