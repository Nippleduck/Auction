using Ardalis.Result;
using Auction.Business.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Business.Interfaces.Services
{
    public interface ILotService
    {
        Task<Result<LotModel>> GetLotInfoByIdAsync(int id, CancellationToken ct);
        Task<Result<IEnumerable<LotModel>>> GetForSaleAsync(CancellationToken ct);
        Task<Result<IEnumerable<LotModel>>> GetCategoryMostPopularLotsWithLimitAsync
            (int categoryId, int limit, CancellationToken ct);
        Task<Result<int>> CreateNewLotAsync(NewLotModel model, CancellationToken ct);
        Task<Result> DeleteLotAsync(int id, CancellationToken ct);
    }
}
