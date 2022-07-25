namespace Auction.Data.QueryFilters
{
    public abstract class PaginatedQueryFilter
    {
        private const int maxPageSize = 24;

        public int PageNumber { get; set; } = 1;

        private int pageSize = 6;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
