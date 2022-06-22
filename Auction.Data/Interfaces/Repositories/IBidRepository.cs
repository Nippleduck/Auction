using Auction.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces.Repositories
{
    public interface IBidRepository : IRepository<Bid, int> 
    {
        Task<Bid> GetHighestBidderAsync(int lotId, CancellationToken ct = default);
    }
}
