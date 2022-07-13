using Microsoft.AspNetCore.Http;

namespace Auction.ApiModels.Reviews.Requests
{
#nullable enable
    public class ReapplyRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? StartPrice { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
