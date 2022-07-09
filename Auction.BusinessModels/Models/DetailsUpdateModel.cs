namespace Auction.BusinessModels.Models
{
    public class DetailsUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; } 
        public int CategoryId { get; set; }
        public int StartPrice { get; set; }
    }
}
