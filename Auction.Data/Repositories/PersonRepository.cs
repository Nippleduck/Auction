using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;


namespace Auction.Data.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(AuctionContext context) : base(context) { }
    }
}
