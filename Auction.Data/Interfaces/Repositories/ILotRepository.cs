using Auction.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces.Repositories
{
    public interface ILotRepository : IRepository<Lot, int>
    {
        Task<Lot> GetByNameAsync(string name, CancellationToken ct);
        Task<IEnumerable<Lot>> GetAllAvailableForSaleAsync(CancellationToken ct);
        Task<IEnumerable<Lot>> GetMostPupularByCategoryWithLimitAsync(int categoryId, int limit, CancellationToken ct);
        Task<IEnumerable<Lot>> GetRequestedForReviewAsync(CancellationToken ct);
        Task<IEnumerable<Lot>> GetUserPurchasedLotsAsync(int userId, CancellationToken ct);
        Task<IEnumerable<Lot>> GetUserSaleLotsAsync(int userId, CancellationToken ct);
    }
}
