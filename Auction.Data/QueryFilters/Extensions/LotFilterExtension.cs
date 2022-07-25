using Auction.Domain.Entities;
using System;
using System.Linq;

namespace Auction.Data.QueryFilters.Extensions
{
    public static class LotFilterExtension
    {

        private const string ByDateAscending = "dateAsc";
        private const string ByDateDescending = "dateDesc";
        private const string ByPriceAscending = "priceAsc";
        private const string ByPriceDescending = "priceDesc";

        public static IQueryable<Lot> WithLotFilter(this IQueryable<Lot> source, LotQueryFilter filter)
        {
            var bySaleStatus = filter.ForSale ? source.Where(lot => DateTime.Now < lot.CloseDate) :
                source.Where(lot => DateTime.Now > lot.CloseDate);

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

            return sorted;
        }
    }
}
