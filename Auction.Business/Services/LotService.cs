using Auction.Business.Interfaces.Services;
using Auction.Business.ImageProcessing;
using Auction.Business.Services.Base;
using Auction.Business.Interfaces;
using Auction.Domain.Entities;
using Auction.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result;
using System.Linq;
using AutoMapper;
using Auction.BusinessModels.Models;

namespace Auction.Business.Services
{
    public class LotService : BaseService, ILotService
    {
        public LotService(IMapper mapper, IUnitOfWork uof, IImageConverter imageConverter)
            : base(mapper, uof) => this.imageConverter = imageConverter;

        private readonly IImageConverter imageConverter;

        public async Task<Result<LotModel>> GetLotInfoByIdAsync(int id, CancellationToken ct)
        {
            var lotInfo = await uof.LotRepository.GetByIdWithDetailsAsync(id, ct);

            if (lotInfo == null) return Result.NotFound();

            var mapped = mapper.Map<LotModel>(lotInfo);
            
            return Result.Success(mapped);
        }

        public async Task<Result<IEnumerable<SaleLotModel>>> GetForSaleAsync(CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetAllAvailableForSaleAsync(ct);

            if (lots == null || !lots.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<SaleLotModel>>(lots);

            return Result.Success(mapped);
        }

        public async Task<Result<IEnumerable<SaleLotModel>>> GetCategoryMostPopularLotsWithLimitAsync
            (int categoryId, int limit, CancellationToken ct)
        {
            var mostPopular = await uof.LotRepository.GetMostPupularByCategoryWithLimitAsync(categoryId, limit, ct);

            if (mostPopular == null || !mostPopular.Any()) return Result.NotFound();

            var mapped = mapper.Map<IEnumerable<SaleLotModel>>(mostPopular);

            return Result.Success(mapped);
        }

        public async Task<Result<int>> CreateNewLotAsync(int sellerId, NewLotModel model, CancellationToken ct)
        {
            using var stream = model.Image.Content;

            var fullSize = await imageConverter.ConvertWithResizeAsync(stream, ImageSize.FullSize);
            var thumbnail = await imageConverter.ConvertWithResizeAsync(stream, ImageSize.Thumbnail);

            var image = new LotImage
            {
                Name = model.Image.FileName,
                Type = model.Image.Type,
                FullSize = fullSize,
                Thumbnail = thumbnail
            };

            var lot = new Lot
            {
                SellerId = sellerId,
                Name = model.Name,
                Description = model.Description,
                StartPrice = model.StartPrice,
                CategoryId = model.CategoryId,
                StatusId = model.StatusId,
                Image = image,
                ReviewDetails = new ReviewDetails()
            };

            await uof.LotRepository.AddAsync(lot, ct);
            await uof.SaveAsync(ct);

            return Result.Success(lot.Id);
        }

        public async Task<Result> DeleteLotAsync(int id, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdAsync(id, ct);

            if (lot == null) return Result.NotFound();

            uof.LotRepository.Delete(lot);
            await uof.SaveAsync(ct);

            return Result.Success();
        }

        public async Task<Result> UpdateLotAsync(LotModel model, CancellationToken ct)
        {
            var lot = mapper.Map<Lot>(model);

            uof.LotRepository.Update(lot);
            await uof.SaveAsync();

            return Result.Success();
        }
    }
}
