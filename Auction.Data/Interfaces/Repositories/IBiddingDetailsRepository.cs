using Auction.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces.Repositories
{
    public interface IBiddingDetailsRepository : IRepository<BiddingDetails, int> 
    {
        Task<BiddingDetails> GetByLotIdAsync(int lotId, CancellationToken ct = default);
    }
}
