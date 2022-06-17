using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class LotRepository : BaseRepository<Lot>, ILotRepository
    {
        public LotRepository(AuctionContext context) : base(context) { }

        public async Task<Lot> GetByNameAsync(string name, CancellationToken ct = default) =>
            await context.Lots  
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .FirstOrDefaultAsync(lot => lot.Name == name, ct);

        public async Task<IEnumerable<Lot>> GetAllAvailableForSaleAsync(CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.Allowed && DateTime.UtcNow < lot.CloseDate)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetMostPupularByCategoryWithLimitAsync
            (int categoryId, int limit, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .Where(lot => lot.CategoryId == categoryId &&
                lot.ReviewDetails.Status == ReviewStatus.Allowed && DateTime.UtcNow < lot.CloseDate)
                .Take(limit)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetRequestedForReviewAsync(CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.ReviewDetails)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.PendingReview)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetUserPurchasedLotsAsync(int userId, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Where(lot => lot.BuyerId == userId)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetUserSaleLotsAsync(int userId, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.BiddingDetails)
                .Include(lot => lot.ReviewDetails)
                .Where(lot => lot.SellerId == userId)
                .ToListAsync(ct);

        public override async Task<Lot> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Lots
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.Buyer)
                .Include(lot => lot.ReviewDetails)
                .FirstOrDefaultAsync(lot => lot.Id == id, ct);

        public override async Task<IEnumerable<Lot>> GetAllWithDetailsAsync(CancellationToken ct = default)
        {
            var res = await context.Lots
                .AsNoTracking()
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.Buyer)
                .Include(lot => lot.ReviewDetails)
                .ToListAsync(ct);

            return res;
        }  
    }
}
