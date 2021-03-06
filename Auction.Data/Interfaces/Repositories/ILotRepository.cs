using Auction.Data.Pagination;
using Auction.Data.QueryFilters;
using Auction.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces.Repositories
{
    public interface ILotRepository : IRepository<Lot, int>
    {
        Task<Lot> GetByNameAsync(string name, CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetAllAvailableForSaleAsync(CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetMostPupularByCategoryWithLimitAsync(int categoryId, int limit, CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetMostPupularWithLimitAsync(int lotId, int limit, CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetRequestedForReviewAsync(CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetUserPurchasedLotsAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetUserSaleLotsAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetUserParticipatedLotsAsync(int userId, CancellationToken ct = default);
        Task<PagedCollection<Lot>> GetByQueryFilterAsync(LotQueryFilter filter, CancellationToken ct = default);
        Task<IEnumerable<Lot>> GetByAdminQueryFiltrerAsync(AdminLotQueryFilter filter, CancellationToken ct = default);
        Task ChangeImageAsync(int id, LotImage image, CancellationToken ct = default);
    }
}
