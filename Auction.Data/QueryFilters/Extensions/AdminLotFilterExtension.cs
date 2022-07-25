using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using System;
using System.Linq;

namespace Auction.Data.QueryFilters.Extensions
{
    public static class AdminLotFilterExtension
    {
        private const string Pending = "pending";
        private const string Completed = "complete";

        public static IQueryable<Lot> WithAdminLotFilter(this IQueryable<Lot> source, AdminLotQueryFilter filter)
        {
            var byStatus = filter.Status switch
            {
                Pending => source.Where(l => l.ReviewDetails.Status == ReviewStatus.PendingReview),
                Completed => source.Where(l => l.CloseDate < DateTime.Now &&
                l.BiddingDetails.Bids.Count > 0 && !l.BiddingDetails.Sold),
                _ => source
            };

            var byName = !string.IsNullOrWhiteSpace(filter.Name) ?
                byStatus.Where(lot => lot.Name.Contains(filter.Name)) : byStatus;

            var categories = !string.IsNullOrEmpty(filter.Categories) ?
                filter.Categories.Trim().Split(",") : Array.Empty<string>();
            var byCategories = categories.Any() ? byName.Where(lot => categories.Contains(lot.Category.Name)) : byName;

            var sellers = !string.IsNullOrEmpty(filter.Sellers) ?
                filter.Sellers.Trim().Split(",").Select(s => s.Replace(" ", "")) : Array.Empty<string>();
            var bySellers = sellers.Any() ? byCategories.Where(lot =>
                sellers.Contains(lot.Seller.Name + lot.Seller.Surname)) : byCategories;

            return bySellers;
        }
    }
}
