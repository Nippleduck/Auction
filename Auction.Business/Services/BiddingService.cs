using Auction.Business.Interfaces.Services;
using Auction.Business.Services.Base;
using Auction.Data.Interfaces;
using Auction.Domain.Entities;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result;
using AutoMapper;

namespace Auction.Business.Services
{
    public class BiddingService : BaseService, IBiddingService
    {
        public BiddingService(IMapper mapper, IUnitOfWork uof) : base(mapper, uof) { }

        public async Task<Result> PlaceBidAsync(int bidder, int lotId, decimal price, CancellationToken ct)
        {
            var details = await uof.BiddingDetailsRepository.GetByLotIdAsync(lotId, ct);

            if (details == null) return Result.NotFound();

            var bid = new Bid
            {
                BidderId = bidder,
                Price = price,
                BiddingDetailsId = details.Id
            };

            await uof.BidRepository.AddAsync(bid, ct);
            await uof.SaveAsync(ct);

            return Result.Success();
        }

        public async Task<Result<Bid>> GetLotHighestBidderAsync(int lotId, CancellationToken ct)
        {
            var bid = await uof.BidRepository.GetHighestBidderAsync(lotId, ct);

            if (bid == null) return Result.NotFound();

            return Result.Success(bid);
        }
    }
}
