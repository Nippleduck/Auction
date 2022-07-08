using Ardalis.Result;
using Auction.BusinessModels.Models;
using Auction.Data.QueryFilters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Business.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Result<IEnumerable<LotDetailedModel>>> GetAllAvailableAsync(CancellationToken ct);
        Task<Result<IEnumerable<LotDetailedModel>>> GetByAdminFilterAsync(AdminLotQueryFilter filter, CancellationToken ct);
        Task<Result<IEnumerable<LotModel>>> GetRequestedForReviewAsync(CancellationToken ct);
        Task<Result> ApproveAsync(int lotId, ReviewApprovalModel model, CancellationToken ct);
        Task<Result> RejectAsync(int lotId, string feedback, CancellationToken ct);
    }
}
