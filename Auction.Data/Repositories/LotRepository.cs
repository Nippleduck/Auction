using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class LotRepository : BaseRepository<Lot>, ILotRepository
    {
        public LotRepository(AuctionContext context) : base(context) { }

        public async Task<Lot> GetByIdWithDetailsAsync(int id, CancellationToken ct) =>
            await context.Lots
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.Buyer)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .FirstOrDefaultAsync(lot => lot.Id == id, ct);

        public async Task<Lot> GetByNameAsync(string name, CancellationToken ct) =>
            await context.Lots  
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .FirstOrDefaultAsync(lot => lot.Name == name, ct);

        public async Task<IEnumerable<Lot>> GetAllAvailableForSaleAsync(CancellationToken ct) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.Allowed && !lot.BiddingDetails.Sold)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetMostPupularByCategoryWithLimitAsync
            (int categoryId, int limit, CancellationToken ct) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .Where(lot => lot.CategoryId == categoryId &&
                lot.ReviewDetails.Status == ReviewStatus.Allowed && !lot.BiddingDetails.Sold)
                .Take(limit)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetRequestedForReviewAsync(CancellationToken ct) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.ReviewDetails)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.PendingReview)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetUserPurchasedLotsAsync(int userId, CancellationToken ct) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Where(lot => lot.BuyerId == userId)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetUserSaleLotsAsync(int userId, CancellationToken ct) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.BiddingDetails)
                .Include(lot => lot.ReviewDetails)
                .Where(lot => lot.SellerId == userId)
                .ToListAsync(ct);
    }
}
