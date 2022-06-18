using Auction.Business.ImageProcessing;
using System.Threading.Tasks;
using System.IO;

namespace Auction.Business.Interfaces
{
    public interface IImageConverter
    {
        Task<byte[]> ConvertWithResizeAsync(Stream stream, ImageSize resize = ImageSize.Initial);
    }
}
