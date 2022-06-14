using Auction.Data.Context;
using Auction.Data.Interfaces;
using Auction.Data.Interfaces.Repositories;
using Auction.Data.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AuctionContext context) => this.context = context;

        private readonly AuctionContext context;

        private IAuctionStatusRepository auctionStatusRepository;
        private IBiddingDetailsRepository biddingDetailsRepository;
        private IBidRepository bidRepository;
        private ICategoryRepository categoryRepository;
        private ILotRepository lotRepository;
        private IPersonRepository personRepository;
        private IReviewDetailsRepository reviewDetailsRepository;

        public IAuctionStatusRepository AuctionStatusRepository => auctionStatusRepository ??= new AuctionStatusRepository(context);

        public IBiddingDetailsRepository BiddingDetailsRepository => biddingDetailsRepository ??= new BiddingDetailsRepository(context);

        public IBidRepository BidRepository => bidRepository ??= new BidRepository(context);

        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(context);    

        public ILotRepository LotRepository => lotRepository ??= new LotRepository(context);

        public IPersonRepository PersonRepository => personRepository ??= new PersonRepository(context);

        public IReviewDetailsRepository ReviewDetailsRepository => reviewDetailsRepository ??= new ReviewDetailsRepository(context);

        public async Task SaveAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct);
    }
}
