using Ardalis.Result;
using Auction.BusinessModels.Models;
using Auction.Data.QueryFilters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Business.Interfaces.Services
{
    public interface ILotService
    {
        Task<Result<LotModel>> GetLotInfoByIdAsync(int id, CancellationToken ct);
        Task<Result<IEnumerable<SaleLotModel>>> GetForSaleAsync(CancellationToken ct);
        Task<Result<IEnumerable<SaleLotModel>>> GetCategoryMostPopularLotsWithLimitAsync
            (int categoryId, int limit, CancellationToken ct);
        Task<Result<IEnumerable<SaleLotModel>>> GetForSaleByFilterAsync(LotQueryFilter filter, CancellationToken ct);
        Task<Result<int>> CreateNewLotAsync(int sellerId, NewLotModel model, CancellationToken ct);
        Task<Result> DeleteLotAsync(int id, CancellationToken ct);
        Task<Result> UpdateLotAsync(LotModel model, CancellationToken ct);
    }
}
