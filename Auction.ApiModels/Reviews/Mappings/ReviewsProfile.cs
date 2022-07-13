using Auction.ApiModels.Reviews.Requests;
using Auction.BusinessModels.Models;
using AutoMapper;

namespace Auction.ApiModels.Reviews.Mappings
{
    public class ReviewsProfile : Profile
    {
        public ReviewsProfile()
        {
            CreateMap<ApprovePlacementRequest, ReviewApprovalModel>();
            CreateMap<ReapplyRequest, ReapplyModel>()
                .ForPath(l => l.Image.FileName, p => p.MapFrom(x => x.Image.FileName))
                .ForPath(l => l.Image.Type, p => p.MapFrom(x => x.Image.ContentType))
                .ForPath(l => l.Image.Content, p => p.MapFrom(x => x.Image.OpenReadStream()));
        }
    }
}
