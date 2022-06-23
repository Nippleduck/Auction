using Ardalis.Result;
using Auction.Business.Interfaces.Services;
using Auction.Business.Services.Base;
using Auction.Data.Interfaces;
using Auction.Domain.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Business.Services
{
    public class ReviewService : BaseService, IReviewService
    {
        public ReviewService(IMapper mapper, IUnitOfWork uof) : base(mapper, uof) { }

        public async Task<Result<IEnumerable<ReviewDetails>>> GetRequestedForReviewAsync(CancellationToken ct)
        {
            var details = await uof.LotRepository.GetRequestedForReviewAsync(ct);

            if (details == null || !details.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<ReviewDetails>>(details);

            return Result.Success(mapped);
        }  

        public async Task<Result> ApproveAsync(int lotId)
        {
            
        }
    }
}
