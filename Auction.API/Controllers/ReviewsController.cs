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
        public async Task<Result<IEnumerable<LotModel>>> Get(CancellationToken ct) =>
            await reviewService.GetRequestedForReviewAsync(ct);

        [HttpPut("approve")]
        [Authorize(Roles = "Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result> Approve([FromBody]ApprovePlacementRequest request, CancellationToken ct) =>
            await reviewService.ApproveAsync(mapper.Map<ReviewApprovalModel>(request), ct);

        [HttpPut("reject")]
        [Authorize(Roles = "Administrator")]
        [TranslateResultToActionResult]
        public async Task<Result> Reject([FromBody]int lotId, string feedback, CancellationToken ct) =>
            await reviewService.RejectAsync(lotId, feedback, ct);
    }
}
