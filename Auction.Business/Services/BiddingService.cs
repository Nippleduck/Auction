using Auction.Business.Interfaces.Services;
using Auction.Business.Services.Base;
using Auction.Data.Interfaces;
using AutoMapper;

namespace Auction.Business.Services
{
    public class BiddingService : BaseService, IBiddingService
    {
        public BiddingService(IMapper mapper, IUnitOfWork uof) : base(mapper, uof)
        {

        }
    }
}
