using System;

namespace Auction.BusinessModels.Models
{
    public class NewAdminLotModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public int MinimalBid { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public ImageModel Image { get; set; }
    }
}
