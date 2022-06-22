using Auction.BusinessModels.Models;
using Auction.Domain.Entities;
using AutoMapper;

namespace Auction.BusinessModels.Mappings
{
    public class BiddingProfile : Profile
    {
        public BiddingProfile()
        {
            CreateMap<Bid, BidModel>();
        }
    }
}
