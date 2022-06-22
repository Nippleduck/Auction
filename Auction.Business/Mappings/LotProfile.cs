using Auction.Business.Models;
using Auction.Domain.Entities;
using AutoMapper;

namespace Auction.Business.Mappings
{
    public class LotProfile : Profile
    {
        public LotProfile()
        {
            CreateMap<Lot, LotModel>()
                .ForMember(l => l.Category, p => p.MapFrom(x => x.Category.Name))
                .ForMember(l => l.Status, p => p.MapFrom(x => x.Status.Name))
                .ForMember(l => l.BidsCount, p => p.MapFrom(x => x.BiddingDetails.BidsCount))
                .ForMember(l => l.MinimalBid, p => p.MapFrom(x => x.BiddingDetails.MinimalBid))
                .ForMember(l => l.CurrentBid, p => p.MapFrom(x => x.BiddingDetails.CurrentBid))
                .ReverseMap()
                .ForPath(l => l.Category.Name, p => p.MapFrom(x => x.Category))
                .ForPath(l => l.Status.Name, p => p.MapFrom(x => x.Status))
                .ForPath(l => l.BiddingDetails.MinimalBid, p => p.MapFrom(x => x.MinimalBid))
                .ForPath(l => l.BiddingDetails.CurrentBid, p => p.MapFrom(x => x.CurrentBid));

            CreateMap<Lot, ListLotModel>()
                .ForMember(l => l.BidsCount, p => p.MapFrom(x => x.BiddingDetails.BidsCount))
                .ForMember(l => l.CurrentBid, p => p.MapFrom(x => x.BiddingDetails.CurrentBid));

        }
    }
}
