﻿using System.IO;

namespace Auction.Business.Models
{
    public class ImageModel
    {
        public string FileName { get; set; }
        public string Type { get; set; }
        public Stream Content { get; set; }
    }
}
