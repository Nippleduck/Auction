using Ardalis.Result;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Auction.Business.Utility
{
    public static class ResultExtensions
    {
        public static Result<IEnumerable<TMapped>> ToMappedCollectionResult<TEntity, TMapped>
            (this IEnumerable<TEntity> collection, IMapper mapper) where TMapped : class
        {
            if (collection == null || !collection.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<TMapped>>(collection);

            return Result.Success(mapped);
        }
    }
}
