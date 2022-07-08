using Auction.BusinessModels.Models;
using Auction.Domain.Entities;
using AutoMapper;

namespace Auction.BusinessModels.Mappings
{
    public class LotProfile : Profile
    {
        public LotProfile()
        {
            CreateMap<Lot, LotModel>()
                .ForMember(l => l.Category, p => p.MapFrom(x => x.Category.Name))
                .ForMember(l => l.Status, p => p.MapFrom(x => x.Status.Name))
                .ForMember(l => l.MinimalBid, p => p.MapFrom(x => x.BiddingDetails.MinimalBid))
                .ForMember(l => l.CurrentBid, p => p.MapFrom(x => x.BiddingDetails.CurrentBid))
                .ForMember(l => l.Seller, p => p.MapFrom(x => $"{x.Seller.Name} {x.Seller.Surname}"))
                .ForMember(l => l.Bids, p => p.MapFrom(x => x.BiddingDetails.Bids))
                .ReverseMap()
                .ForPath(l => l.Category.Name, p => p.MapFrom(x => x.Category))
                .ForPath(l => l.Status.Name, p => p.MapFrom(x => x.Status))
                .ForPath(l => l.BiddingDetails.MinimalBid, p => p.MapFrom(x => x.MinimalBid))
                .ForPath(l => l.BiddingDetails.CurrentBid, p => p.MapFrom(x => x.CurrentBid));

            CreateMap<Lot, LotDetailedModel>()
                .ForMember(l => l.Category, p => p.MapFrom(x => x.Category.Name))
                .ForMember(l => l.Status, p => p.MapFrom(x => x.Status.Name))
                .ForMember(l => l.MinimalBid, p => p.MapFrom(x => x.BiddingDetails.MinimalBid))
                .ForMember(l => l.CurrentBid, p => p.MapFrom(x => x.BiddingDetails.CurrentBid))
                .ForMember(l => l.Seller, p => p.MapFrom(x => $"{x.Seller.Name} {x.Seller.Surname}"))
                .ForMember(l => l.Bids, p => p.MapFrom(x => x.BiddingDetails.Bids))
                .ForMember(l => l.ReviewStatus, p => p.MapFrom(x => x.ReviewDetails.Status.ToString()))
                .ForMember(l => l.Feedback, p => p.MapFrom(x => x.ReviewDetails.Feedback))
                .ForMember(l => l.Sold, p => p.MapFrom(x => x.BiddingDetails.Sold))
                .ReverseMap()
                .ForPath(l => l.Category.Name, p => p.MapFrom(x => x.Category))
                .ForPath(l => l.Status.Name, p => p.MapFrom(x => x.Status))
                .ForPath(l => l.BiddingDetails.MinimalBid, p => p.MapFrom(x => x.MinimalBid))
                .ForPath(l => l.BiddingDetails.CurrentBid, p => p.MapFrom(x => x.CurrentBid));

            CreateMap<Lot, SaleLotModel>()
                .ForMember(l => l.CurrentBid, p => p.MapFrom(x => x.BiddingDetails.CurrentBid))
                .ForMember(l => l.Category, p => p.MapFrom(x => x.Category.Name));

            CreateMap<Category, CategoryModel>();
            CreateMap<AuctionStatus, StatusModel>();
        }
    }
}
