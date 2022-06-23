using Auction.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces.Repositories
{
    public interface IReviewDetailsRepository : IRepository<ReviewDetails, int> 
    {
        Task<ReviewDetails> GetByLotIdAsync(int lotId, CancellationToken ct = default);
    }
}
