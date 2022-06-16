using Auction.Data.Interfaces.Repositories;
using Auction.Domain.Entities.Base;
using Auction.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : BaseEntity 
    {
        protected BaseRepository(AuctionContext context) => this.context = context;

        protected readonly AuctionContext context;

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default) =>
            await context.Set<TEntity>().ToListAsync(ct);

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default) =>
            await context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task AddAsync(TEntity entity, CancellationToken ct = default) =>
            await context.Set<TEntity>().AddAsync(entity, ct);

        public async Task DeleteByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, ct);
            
            context.Set<TEntity>().Remove(entity);
        }

        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => context.Entry(entity).State = EntityState.Modified;

        public abstract Task<TEntity> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);

        public abstract Task<IEnumerable<TEntity>> GetAllWithDetailsAsync(CancellationToken ct = default);
    }
}
