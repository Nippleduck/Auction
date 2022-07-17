using Ardalis.Result;
using Auction.BusinessModels.Models;
using Auction.Data.QueryFilters;
using System;
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
        Task<Result<IEnumerable<SaleLotModel>>> GetMostPopularWithLimitAsync(int limit, CancellationToken ct);
        Task<Result<IEnumerable<SaleLotModel>>> GetForSaleByFilterAsync(LotQueryFilter filter, CancellationToken ct);
        Task<Result<int>> CreateLotAsync(int sellerId, NewLotModel model, CancellationToken ct);
        Task<Result<int>> CreateLotAsAdminAsync(int sellerId, NewAdminLotModel model, CancellationToken ct);
        Task<Result> DeleteLotAsync(int id, CancellationToken ct);
        Task<Result> UpdateLotAsync(LotModel model, CancellationToken ct);
        Task<Result<DateTime>> BeginAuctionAsync(int id, CancellationToken ct);
        Task<Result<DateTime>> CloseAuctionAsync(int id, CancellationToken ct);
        Task<Result> UpdateDetailsAsync(int id, DetailsUpdateModel model, CancellationToken ct);
        Task<Result<IEnumerable<CategoryModel>>> GetCategoriesAsync(CancellationToken ct);
        Task<Result<IEnumerable<StatusModel>>> GetStatusesAsync(CancellationToken ct);
        Task<Result> UpdateBiddingDetailsAsync(int id, BiddingDetailsUpdateModel model, CancellationToken ct);
        Task<Result> UpdateStatusAsync(int lotId, int statusId, CancellationToken ct);
        Task<Result<IEnumerable<UserLotSaleModel>>> GetUserParticipatedLotsAsync(int userId, CancellationToken ct);
        Task<Result<IEnumerable<UserLotSaleModel>>> GetUserOwnedLotsAsync(int userId, CancellationToken ct);
    }
}
