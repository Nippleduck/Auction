using Auction.API.Controllers.Base;
using Auction.API.CurrentUserService;
using Auction.ApiModels.Reviews.Requests;
using Auction.Business.Interfaces.Services;
using Auction.BusinessModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using AutoMapper;
using Auction.Data.QueryFilters;

namespace Auction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : BaseController
    {
        public ReviewsController(CurrentUserAccessor currentUser, IMapper mapper, IReviewService reviewService)
            : base(currentUser, mapper) => this.reviewService = reviewService;

        private readonly IReviewService reviewService;

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result<IEnumerable<LotDetailedModel>>> GetAll(CancellationToken ct, [FromQuery]AdminLotQueryFilter filter = null) =>
            filter == null ? await reviewService.GetAllAvailableAsync(ct) 
            : await reviewService.GetByAdminFilterAsync(filter, ct);

        [HttpGet("reviews")]
        [Authorize(Roles = "Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result<IEnumerable<LotModel>>> Get(CancellationToken ct) =>
            await reviewService.GetRequestedForReviewAsync(ct);

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result> Approve(int id, [FromBody]ApprovePlacementRequest request, CancellationToken ct) =>
            await reviewService.ApproveAsync(id, mapper.Map<ReviewApprovalModel>(request), ct);

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result> Reject(int id, [FromBody]string feedback, CancellationToken ct) =>
            await reviewService.RejectAsync(id, feedback, ct);

        [HttpPut("{id}/reapply")]
        [Authorize(Roles = "Customer")]
        [TranslateResultToActionResult]
        public async Task<Result> Reapply(int id, [FromForm] ReapplyRequest request, CancellationToken ct) =>
            await reviewService.ReapplyAsync(id, mapper.Map<ReapplyModel>(request), ct);
    }
}
