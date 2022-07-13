using Auction.Business.ImageProcessing;
using Auction.Business.Interfaces;
using Auction.BusinessModels.Models;
using Auction.Domain.Entities;
using System.Threading.Tasks;

namespace Auction.Business.Utility
{
    public static class ImageMappingExtensions
    {
        public static async Task<LotImage> ToDbStoredImageAsync(this ImageModel imageModel, IImageConverter converter)
        {
            using var stream = imageModel.Content;

            var fullSize = await converter.ConvertWithResizeAsync(stream, ImageSize.FullSize);
            var thumbnail = await converter.ConvertWithResizeAsync(stream, ImageSize.Thumbnail);

            var image = new LotImage
            {
                Name = imageModel.FileName,
                Type = imageModel.Type,
                FullSize = fullSize,
                Thumbnail = thumbnail
            };

            return image;
        }
    }
}
