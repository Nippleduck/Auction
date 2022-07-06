using Ardalis.Result;
using Auction.Domain.Entities.Base;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Auction.Business.Utility
{
    public static class ResultExtensions
    {
        public static Result<IEnumerable<TMapped>> ToMappedCollectionResult<TEntity, TMapped>
            (this IEnumerable<TEntity> collection, IMapper mapper) 
            where TMapped : class 
            where TEntity : BaseEntity
        {
            if (collection == null || !collection.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<TMapped>>(collection);

            return Result.Success(mapped);
        }

        public static Result<TMapped> ToMappedResult<TEntity, TMapped>(this TEntity entity, IMapper mapper)
            where TMapped : class
            where TEntity : BaseEntity
        {
            if (entity == null) return Result.NotFound();

            var mapped = mapper.Map<TMapped>(entity);

            return Result.Success(mapped);
        }
    }
}
