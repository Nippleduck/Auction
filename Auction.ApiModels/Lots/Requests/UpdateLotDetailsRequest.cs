namespace Auction.ApiModels.Lots.Requests
{
    public class UpdateLotDetailsRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
