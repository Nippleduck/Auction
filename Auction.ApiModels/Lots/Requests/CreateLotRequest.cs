using Microsoft.AspNetCore.Http;

namespace Auction.ApiModels.Lots.Requests
{
    public class CreateLotRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public IFormFile Image { get; set; }
    }
}
