using Ardalis.Result;
using Auction.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Business.Interfaces.Services
{
    public interface IBiddingService
    {
        Task<Result> PlaceBidAsync(int bidder, int lotId, decimal price, CancellationToken ct);
        Task<Result<Bid>> GetLotHighestBidderAsync(int lotId, CancellationToken ct);
    }
}
