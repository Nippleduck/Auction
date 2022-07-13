namespace Auction.BusinessModels.Models
{
#nullable enable
    public class ReapplyModel
    {
        public ImageModel? Image { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? StartPrice { get; set; }
    }
}
