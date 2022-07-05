using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Data.QueryFilters;
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
                .Include(lot => lot.BiddingDetails)
                .Where(lot => lot.BiddingDetails.BuyerId == userId)
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

        private const string ByDateAscending = "dateAsc";
        private const string ByDateDescending = "dateDesc";
        private const string ByPriceAscending = "priceAsc";
        private const string ByPriceDescending = "priceDesc";

        public async Task<IEnumerable<Lot>> GetByQueryFilterAsync(LotQueryFilter filter, CancellationToken ct = default)
        {
            var included = context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.Allowed);

            var bySaleStatus = filter.ForSale ? included.Where(lot => DateTime.UtcNow < lot.CloseDate) :
                included.Where(lot => DateTime.UtcNow > lot.CloseDate);

            var byName = !string.IsNullOrWhiteSpace(filter.LotName) ?
                bySaleStatus.Where(lot => lot.Name.Contains(filter.LotName)) : bySaleStatus;

            var categories = !string.IsNullOrEmpty(filter.Categories) ?
                filter.Categories.Trim().Split(",") : Array.Empty<string>();
            var byCategories = categories.Any() ? byName.Where(lot => categories.Contains(lot.Category.Name)) : byName;

            var byMinPrice = filter.MinPrice != default ?
                byCategories.Where(lot => lot.StartPrice >= filter.MinPrice) : byCategories;

            var byMaxPrice = filter.MaxPrice != default ? 
                byMinPrice.Where(lot => lot.StartPrice <= filter.MaxPrice) : byMinPrice;

            var sorted = filter.SortBy switch
            {
                ByDateAscending => byMaxPrice.OrderBy(lot => lot.CloseDate),
                ByDateDescending => byMaxPrice.OrderByDescending(lot => lot.CloseDate),
                ByPriceAscending => byMaxPrice.OrderBy(lot => lot.StartPrice),
                ByPriceDescending => byMaxPrice.OrderByDescending(lot => lot.StartPrice),
                _ => byMaxPrice
            };

            return await sorted.ToListAsync(ct);
        }

        public override async Task<Lot> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Lots
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .ThenInclude(b => b.Bidder)
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.ReviewDetails)
                .FirstOrDefaultAsync(lot => lot.Id == id, ct);

        public override async Task<IEnumerable<Lot>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .ThenInclude(b => b.Bidder)
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.ReviewDetails)
                .ToListAsync(ct);
    }
}
