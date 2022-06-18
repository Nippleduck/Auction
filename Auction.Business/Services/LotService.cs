using Auction.Business.Services.Base;
using Auction.Business.Models;
using Auction.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result;
using System.Linq;
using AutoMapper;

namespace Auction.Business.Services
{
    public class LotService : BaseService
    {
        public LotService(IMapper mapper, IUnitOfWork uof) : base(mapper, uof) { }

        public async Task<Result<LotModel>> GetLotInfoByIdAsync(int id, CancellationToken ct)
        {
            var lotInfo = await uof.LotRepository.GetByIdWithDetailsAsync(id, ct);

            if (lotInfo == null) return Result.NotFound();

            var mapped = mapper.Map<LotModel>(lotInfo);

            return Result.Success(mapped);
        }

        public async Task<Result<IEnumerable<LotModel>>> GetForSaleAsync(CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetAllAvailableForSaleAsync(ct);

            if (lots == null || !lots.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<LotModel>>(lots);

            return Result.Success(mapped);
        }

        public async Task<Result<IEnumerable<LotModel>>> GetCategoryMostPopularLotsWithLimitAsync
            (int categoryId, int limit, CancellationToken ct)
        {
            var mostPopular = await uof.LotRepository.GetMostPupularByCategoryWithLimitAsync(categoryId, limit, ct);

            if (mostPopular == null || !mostPopular.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<LotModel>>(mostPopular);

            return Result.Success(mapped);
        }

        public async Task<Result<bool>> CreateLotAsync()
    }
}
