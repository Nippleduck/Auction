using Auction.BusinessModels.Models;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result;

namespace Auction.Business.Interfaces.Services
{
    public interface IBiddingService
    {
        Task<Result<int>> PlaceBidAsync(int bidder, int lotId, decimal price, CancellationToken ct);
        Task<Result<BidModel>> GetLotHighestBidderAsync(int lotId, CancellationToken ct);
    }
}
