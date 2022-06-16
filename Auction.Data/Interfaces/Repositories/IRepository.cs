using Auction.Domain.Entities.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Data.Interfaces.Repositories
{
    public interface IRepository<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TEntity> GetByIdAsync(TId id, CancellationToken ct = default);
        Task<TEntity> GetByIdWithDetailsAsync(TId id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        Task DeleteByIdAsync(TId id, CancellationToken ct = default);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
