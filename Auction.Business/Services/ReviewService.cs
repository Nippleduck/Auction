using Auction.Business.Interfaces.Services;
using Auction.Business.Services.Base;
using Auction.BusinessModels.Models;
using Auction.Domain.Entities.Enums;
using Auction.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Ardalis.Result;
using AutoMapper;

namespace Auction.Business.Services
{
    public class ReviewService : BaseService, IReviewService
    {
        public ReviewService(IMapper mapper, IUnitOfWork uof) : base(mapper, uof) { }

        public async Task<Result<IEnumerable<LotModel>>> GetAllAvailableAsync(CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetAllWithDetailsAsync(ct);

            if (lots == null || !lots.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<LotModel>>(lots);

            return Result.Success(mapped);
        }

        public async Task<Result<IEnumerable<LotModel>>> GetRequestedForReviewAsync(CancellationToken ct)
        {
            var details = await uof.LotRepository.GetRequestedForReviewAsync(ct);

            if (details == null || !details.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<LotModel>>(details);

            return Result.Success(mapped);
        }  

        public async Task<Result> ApproveAsync(ReviewApprovalModel model, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(model.LotId, ct);

            if (lot == null) return Result.NotFound();

            lot.OpenDate = model.OpenDate;
            lot.CloseDate = model.CloseDate;
            lot.StatusId = model.StatusId;

            lot.ReviewDetails.Status = ReviewStatus.Allowed;
            lot.ReviewDetails.Feedback = model.Feedback;

            uof.LotRepository.Update(lot);
            await uof.SaveAsync(ct);

            return Result.SuccessWithMessage($"Lot: ({lot.Name}) approved");
        }

        public async Task<Result> RejectAsync(int lotId, string feedback, CancellationToken ct)
        {
            var details = await uof.ReviewDetailsRepository.GetByLotIdAsync(lotId, ct);

            if (details == null) return Result.NotFound();

            details.Status = ReviewStatus.Rejected;
            details.Feedback = feedback;

            uof.ReviewDetailsRepository.Update(details);
            await uof.SaveAsync(ct);

            return Result.Success();
        }
    }
}
