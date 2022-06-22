using Auction.API.CurrentUserService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Auction.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected BaseController(CurrentUserAccessor currentUser, IMapper mapper)
        {
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        protected readonly CurrentUserAccessor currentUser;
        protected readonly IMapper mapper; 
    }
}
