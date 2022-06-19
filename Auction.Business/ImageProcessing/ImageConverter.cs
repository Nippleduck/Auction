using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using Auction.Business.Interfaces;
using System.Threading.Tasks;
using System.IO;

namespace Auction.Business.ImageProcessing
{
    public enum ImageSize
    {
        Initial,
        FullSize = 1000,
        Thumbnail = 400
    }

    public class ImageConverter : IImageConverter
    {
        public async Task<byte[]> ConvertWithResizeAsync(Stream stream, ImageSize resize = ImageSize.Initial)
        {
            using var loaded = await Image.LoadAsync(stream);

            return await Resize(loaded, resize);
        }

        private async Task<byte[]> Resize(Image image,  ImageSize resizeWidth)
        {
            var width = image.Width;
            var height = image.Height;
 
            if (width > (int)resizeWidth && resizeWidth <= ImageSize.Initial)
            {
                height = (int)((double)resizeWidth / width * height);
                width = (int)resizeWidth;
            }

            image.Mutate(i => i.Resize(new Size(width, height)));
            image.Metadata.ExifProfile = null;

            using var stream = new MemoryStream();

            await image.SaveAsJpegAsync(stream, new JpegEncoder { Quality = 75 });

            return stream.ToArray();
        }
    }
}
