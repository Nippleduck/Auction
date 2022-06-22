using Auction.API.CurrentUserService;
using Microsoft.AspNetCore.Mvc;

namespace Auction.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected BaseController(CurrentUserAccessor currentUser) => this.currentUser = currentUser;

        protected readonly CurrentUserAccessor currentUser;
    }
}
