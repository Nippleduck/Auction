﻿namespace Auction.BusinessModels.Models
{
    public class NewLotModel
    {
        public int SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartPrice { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public ImageModel Image { get; set; }
    }
}