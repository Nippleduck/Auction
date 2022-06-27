namespace Auction.Data.QueryFilters
{
    public class LotQueryFilter
    {
        public bool ForSale { get; set; } = true;
        public string LotName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
