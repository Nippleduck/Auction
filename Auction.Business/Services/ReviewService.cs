using Auction.Business.Interfaces.Services;
using Auction.Business.Services.Base;
using Auction.BusinessModels.Models;
using Auction.Domain.Entities.Enums;
using Auction.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result;
using AutoMapper;
using Auction.Data.QueryFilters;
using Auction.Business.Utility;
using Auction.Domain.Entities;
using Auction.Business.Interfaces;
using System;

namespace Auction.Business.Services
{
    public class ReviewService : BaseService, IReviewService
    {
        public ReviewService(IMapper mapper, IUnitOfWork uof, IImageConverter imageConverter) 
            : base(mapper, uof) => this.imageConverter = imageConverter;

        private readonly IImageConverter imageConverter;

        public async Task<Result<IEnumerable<LotDetailedModel>>> GetAllAvailableAsync(CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetAllWithDetailsAsync(ct);

            return lots.ToMappedCollectionResult<Lot, LotDetailedModel>(mapper);
        }

        public async Task<Result<IEnumerable<LotDetailedModel>>> GetByAdminFilterAsync(AdminLotQueryFilter filter, CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetByAdminQueryFiltrerAsync(filter, ct);

            return lots.ToMappedCollectionResult<Lot, LotDetailedModel>(mapper);
        }

        public async Task<Result<IEnumerable<LotModel>>> GetRequestedForReviewAsync(CancellationToken ct)
        {
            var details = await uof.LotRepository.GetRequestedForReviewAsync(ct);

            return details.ToMappedCollectionResult<Lot, LotModel>(mapper);
        }  

        public async Task<Result> ApproveAsync(int lotId, ReviewApprovalModel model, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(lotId, ct);

            if (lot == null) return Result.NotFound();

            lot.OpenDate = model.OpenDate;
            lot.CloseDate = model.CloseDate;
            lot.StatusId = model.StatusId;

            lot.BiddingDetails.MinimalBid = model.MinimalBid;

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

        public async Task<Result> ReapplyAsync(int lotId, ReapplyModel model, CancellationToken ct) 
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(lotId, ct);
            
            if (lot == null) return Result.NotFound();

            if (model.Image.Content != null)
            {
                var image = await model.Image.ToDbStoredImageAsync(imageConverter);
                await uof.LotRepository.ChangeImageAsync(lotId, image, ct);
            }

            if (model.Name != null && !string.IsNullOrWhiteSpace(model.Name)) 
                lot.Name = model.Name;

            if (model.Description != null && !string.IsNullOrWhiteSpace(model.Description)) 
                lot.Description = model.Description;

            if (model.StartPrice != null && model.StartPrice != 0)
                lot.StartPrice = model.StartPrice.Value;

            if (model.CategoryId != null && model.CategoryId != 0)
                lot.CategoryId = model.CategoryId.Value;

            lot.ReviewDetails.Status = ReviewStatus.PendingReview;

            uof.LotRepository.Update(lot);
            await uof.SaveAsync();

            return Result.Success();
        }
    }
}
