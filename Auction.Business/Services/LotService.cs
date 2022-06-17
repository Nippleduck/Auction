using Ardalis.Result;
using Auction.Business.Services.Base;
using Auction.Data.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace Auction.Business.Services
{
    public class LotService : BaseService
    {
        public LotService(IMapper mapper, IUnitOfWork uof) : base(mapper, uof) { }

        public async Task<Result<>>
    }
}
