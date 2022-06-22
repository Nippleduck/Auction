using Auction.Business.Models;
using Auction.Domain.Entities;
using AutoMapper;

namespace Auction.Business.Mappings
{
    public class BiddingProfile : Profile
    {
        public BiddingProfile()
        {
            CreateMap<Bid, BidModel>();
        }
    }
}
