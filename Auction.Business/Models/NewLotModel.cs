namespace Auction.Business.Models
{
    public class NewLotModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public ImageModel Image { get; set; }
    }
}
