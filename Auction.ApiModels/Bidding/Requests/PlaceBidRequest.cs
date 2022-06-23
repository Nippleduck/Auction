namespace Auction.ApiModels.Bidding.Requests
{
    public class PlaceBidRequest
    {
        public int LotId { get; set; }
        public int Price { get; set; }
    }
}
