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
        }
    }
}
