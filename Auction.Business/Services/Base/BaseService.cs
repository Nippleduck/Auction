using Auction.Data.Interfaces;
using AutoMapper;

namespace Auction.Business.Services.Base
{
    public abstract class BaseService
    {
        protected BaseService(IMapper mapper, IUnitOfWork uof)
        {
            this.mapper = mapper;
            this.uof = uof;
        }

        protected readonly IMapper mapper;
        protected readonly IUnitOfWork uof;
    }
}
