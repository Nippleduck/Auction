using System.IO;
using System.Threading.Tasks;

namespace Auction.Business.ImageProcessing
{
    public interface IImageConverter
    {
        Task<byte[]> ConvertWithResizeAsync(Stream stream, ImageSize resize = ImageSize.Initial);
    }
}
