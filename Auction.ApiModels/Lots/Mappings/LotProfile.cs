using Auction.ApiModels.Lots.Requests;
using Auction.BusinessModels.Models;
using AutoMapper;

namespace Auction.ApiModels.Lots.Mappings
{
    public class LotProfile : Profile
    {
        public LotProfile()
        {
            CreateMap<CreateLotRequest, NewLotModel>()
                .ForPath(l => l.Image.FileName, p => p.MapFrom(x => x.Image.FileName))
                .ForPath(l => l.Image.Type, p => p.MapFrom(x => x.Image.ContentType))
                .ForPath(l => l.Image.Content, p => p.MapFrom(x => x.Image.OpenReadStream()));

            CreateMap<CreateAdminLotRequest, NewAdminLotModel>()
                .ForPath(l => l.Image.FileName, p => p.MapFrom(x => x.Image.FileName))
                .ForPath(l => l.Image.Type, p => p.MapFrom(x => x.Image.ContentType))
                .ForPath(l => l.Image.Content, p => p.MapFrom(x => x.Image.OpenReadStream()));

            CreateMap<UpdateLotDetailsRequest, DetailsUpdateModel>();

            CreateMap<UpdateBiddingDetailsRequest, BiddingDetailsUpdateModel>();
        }
    }
}
