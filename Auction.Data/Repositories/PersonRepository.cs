using Auction.Data.Context;
using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(AuctionContext context) : base(context) { }

        public override async Task<Person> GetByIdWithDetailsAsync(int id, CancellationToken ct = default) =>
            await context.Persons
                .Include(p => p.Bids)
                .Include(p => p.PurchasedLots)
                .Include(p => p.OwnedLots)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

        public override async Task<IEnumerable<Person>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
            await context.Persons
                .AsNoTracking()
                .Include(p => p.Bids)
                .Include(p => p.PurchasedLots)
                .Include(p => p.OwnedLots)
                .ToListAsync(ct);
    }
}
