using Auction.Data.Context;
using Auction.Data.Exceptions;
using Auction.Data.Interfaces.Repositories;
using Auction.Data.Pagination;
using Auction.Data.QueryFilters;
using Auction.Data.QueryFilters.Extensions;
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
                .ThenInclude(lot => lot.Bids)
                .Where(lot => lot.CategoryId == categoryId &&
                lot.ReviewDetails.Status == ReviewStatus.Allowed && DateTime.UtcNow < lot.CloseDate)
                .OrderByDescending(lot => lot.BiddingDetails.Bids.Count)
                .Take(limit)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetMostPupularWithLimitAsync (int lotId, int limit, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(lot => lot.Bids)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.Allowed && DateTime.UtcNow < lot.CloseDate)
                .Where(lot => lot.Id != lotId)
                .OrderByDescending(lot => lot.BiddingDetails.Bids.Count)
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
                .ThenInclude(bd => bd.Bids)
                .ThenInclude(b => b.Bidder)
                .Include(lot => lot.ReviewDetails)
                .Where(lot => lot.SellerId == userId)
                .ToListAsync(ct);

        public async Task<IEnumerable<Lot>> GetUserParticipatedLotsAsync(int userId, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .ThenInclude(b => b.Bidder)
                .Include(lot => lot.ReviewDetails)
                .Where(lot => lot.BiddingDetails.Bids.Any(b => b.BidderId == userId))
                .ToListAsync(ct);

        public async Task<PagedCollection<Lot>> GetByQueryFilterAsync(LotQueryFilter filter, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.ReviewDetails)
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(d => d.Bids)
                .Where(lot => lot.ReviewDetails.Status == ReviewStatus.Allowed)
                .WithLotFilter(filter)
                .ToPagedCollectionAsync(filter.PageNumber, filter.PageSize, paged => paged.ToListAsync(ct));

        public async Task<IEnumerable<Lot>> GetByAdminQueryFiltrerAsync(AdminLotQueryFilter filter, CancellationToken ct = default) =>
            await context.Lots
                .AsNoTracking()
                .Include(lot => lot.BiddingDetails)
                .ThenInclude(bd => bd.Bids)
                .ThenInclude(b => b.Bidder)
                .Include(lot => lot.Category)
                .Include(lot => lot.Status)
                .Include(lot => lot.Seller)
                .Include(lot => lot.ReviewDetails)
                .WithAdminLotFilter(filter)
                .ToListAsync(ct);

        public async Task ChangeImageAsync(int id, LotImage image, CancellationToken ct = default)
        {
            var current = await context.Images.Where(i => i.LotId == id).Select(i => i.Id).FirstOrDefaultAsync(ct);

            if (current == 0) throw new EntityNotFoundException("Image does not exist");

            context.Images.Remove(new LotImage { Id = current });

            image.LotId = id;
            context.Entry(image).State = EntityState.Added;
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
