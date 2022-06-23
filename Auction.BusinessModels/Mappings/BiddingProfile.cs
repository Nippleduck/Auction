using Auction.BusinessModels.Models;
using Auction.Domain.Entities;
using AutoMapper;

namespace Auction.BusinessModels.Mappings
{
    public class BiddingProfile : Profile
    {
        public BiddingProfile()
        {
            CreateMap<Bid, BidModel>()
                .ForMember(b => b.Bidder, p => p.MapFrom(x => $"{x.Bidder.Name} {x.Bidder.Surname}"));
        }
    }
}
