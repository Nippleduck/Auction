using Auction.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.ImageStore
{
    public class ImageStoreService
    {
        public ImageStoreService(AuctionContext context) => this.context = context;
        
        private readonly AuctionContext context;

        public async Task<byte[]> GetFullSizeImageAsync(int id, CancellationToken ct) =>
            await context.Images
                .AsNoTracking()
                .Where(i => i.LotId == id)
                .Select(i => i.FullSize)
                .FirstOrDefaultAsync(ct);

        public async Task<byte[]> GetThumbnailImageAsync(int id, CancellationToken ct) =>
            await context.Images
                .AsNoTracking()
                .Where(i => i.LotId == id)
                .Select(i => i.Thumbnail)
                .FirstOrDefaultAsync(ct);
    }
}
