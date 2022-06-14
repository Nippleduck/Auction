using Auction.Data.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IAuctionStatusRepository AuctionStatusRepository { get; }
        IBiddingDetailsRepository BiddingDetailsRepository { get; }
        IBidRepository BidRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ILotRepository LotRepository { get; }
        IPersonRepository PersonRepository { get; }
        IReviewDetailsRepository ReviewDetailsRepository { get; }

        Task SaveAsync(CancellationToken ct = default);
    }
}
