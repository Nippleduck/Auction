using Auction.Business.Interfaces.Services;
using Auction.Business.Services.Base;
using Auction.Business.Interfaces;
using Auction.Business.Utility;
using Auction.BusinessModels.Models;
using Auction.Domain.Entities.Enums;
using Auction.Domain.Entities;
using Auction.Data.QueryFilters;
using Auction.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Result;
using AutoMapper;
using System;

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

            return lotInfo.ToMappedResult<Lot, LotModel>(mapper);
        }

        public async Task<Result<IEnumerable<SaleLotModel>>> GetForSaleAsync(CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetAllAvailableForSaleAsync(ct);

            return lots.ToMappedCollectionResult<Lot, SaleLotModel>(mapper);
        }

        public async Task<Result<IEnumerable<SaleLotModel>>> GetForSaleByFilterAsync(LotQueryFilter filter, CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetByQueryFilterAsync(filter, ct);

            return lots.ToMappedCollectionResult<Lot, SaleLotModel>(mapper);
        }

        public async Task<Result<IEnumerable<SaleLotModel>>> GetCategoryMostPopularLotsWithLimitAsync
            (int categoryId, int limit, CancellationToken ct)
        {
            var mostPopular = await uof.LotRepository.GetMostPupularByCategoryWithLimitAsync(categoryId, limit, ct);

            return mostPopular.ToMappedCollectionResult<Lot, SaleLotModel>(mapper);
        }

        public async Task<Result<IEnumerable<CategoryModel>>> GetCategoriesAsync(CancellationToken ct)
        {
            var categories = await uof.CategoryRepository.GetAllAsync(ct);

            return categories.ToMappedCollectionResult<Category, CategoryModel>(mapper);
        }

        public async Task<Result<IEnumerable<StatusModel>>> GetStatusesAsync(CancellationToken ct)
        {
            var statuses = await uof.AuctionStatusRepository.GetAllAsync(ct);

            return statuses.ToMappedCollectionResult<AuctionStatus, StatusModel>(mapper);
        }

        public async Task<Result<IEnumerable<UserLotSaleModel>>> GetUserParticipatedLotsAsync(int userId, CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetUserParticipatedLotsAsync(userId, ct);

            return lots.ToMappedCollectionResult<Lot, UserLotSaleModel>(mapper);
        }

        public async Task<Result<IEnumerable<UserLotSaleModel>>> GetUserOwnedLotsAsync(int userId, CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetUserSaleLotsAsync(userId, ct);

            return lots.ToMappedCollectionResult<Lot, UserLotSaleModel>(mapper);
        }

        public async Task<Result<IEnumerable<SaleLotModel>>> GetMostPopularWithLimitAsync(int lotId, int limit, CancellationToken ct)
        {
            var lots = await uof.LotRepository.GetMostPupularWithLimitAsync(lotId, limit, ct);

            return lots.ToMappedCollectionResult<Lot, SaleLotModel>(mapper);
        }

        public async Task<Result<int>> CreateLotAsync(int sellerId, NewLotModel model, CancellationToken ct)
        {
            if (model == null) return Result.Error("Mapped model cannot be null");

            var image = await model.Image.ToDbStoredImageAsync(imageConverter);

            var lot = new Lot
            {
                SellerId = sellerId,
                Name = model.Name,
                Description = model.Description,
                StartPrice = model.StartPrice,
                CategoryId = model.CategoryId,
                Image = image,
                ReviewDetails = new ReviewDetails(),
                BiddingDetails = new BiddingDetails()
            };

            await uof.LotRepository.AddAsync(lot, ct);
            await uof.SaveAsync(ct);

            return Result.Success(lot.Id);
        }

        public async Task<Result<int>> CreateLotAsAdminAsync(int sellerId, NewAdminLotModel model, CancellationToken ct)
        {
            if (model == null) return Result.Error("Mapped model cannot be null");

            var image = await model.Image.ToDbStoredImageAsync(imageConverter);

            var lot = new Lot
            {
                SellerId = sellerId,
                Name = model.Name,
                Description = model.Description,
                StartPrice = model.StartPrice,
                CategoryId = model.CategoryId,
                StatusId = model.StatusId,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate,
                Image = image,
                ReviewDetails = new ReviewDetails { Status = ReviewStatus.Allowed },
                BiddingDetails = new BiddingDetails { MinimalBid = model.MinimalBid },
            };

            await uof.LotRepository.AddAsync(lot, ct);
            await uof.SaveAsync(ct);

            return Result.Success(lot.Id);
        }

        public async Task<Result> DeleteLotAsync(int id, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(id, ct);

            if (lot == null) return Result.NotFound();

            if (lot.BiddingDetails.BidsCount > 0 && !lot.BiddingDetails.Sold) return Result.Error(
                "Cannot remove bid if has any bidders with no resolved buyer");

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

        public async Task<Result<DateTime>> BeginAuctionAsync(int id, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(id, ct);

            if (lot == null) return Result.NotFound();

            if (lot.BiddingDetails.Sold || lot.OpenDate < DateTime.Now) return Result.Error();

            lot.OpenDate = DateTime.Now;

            uof.LotRepository.Update(lot);
            await uof.SaveAsync();

            return Result.Success(lot.OpenDate);
        }

        public async Task<Result<DateTime>> CloseAuctionAsync(int id, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(id, ct);

            if (lot == null) return Result.NotFound();

            if (lot.OpenDate > DateTime.Now || lot.CloseDate < DateTime.Now) return Result.Error();

            lot.CloseDate = DateTime.Now;

            uof.LotRepository.Update(lot);
            await uof.SaveAsync(ct);

            return Result.Success(lot.CloseDate);
        }

        public async Task<Result> UpdateDetailsAsync(int id, DetailsUpdateModel model, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdAsync(id, ct);

            if (lot == null) return Result.NotFound();

            lot.Name = model.Title;
            lot.Description = model.Description;
            lot.CategoryId = model.CategoryId;
            
            if (lot.OpenDate > DateTime.Now)
            {
                lot.StartPrice = model.StartPrice;
            }

            uof.LotRepository.Update(lot);
            await uof.SaveAsync(ct);

            return Result.Success();
        }

        public async Task<Result> UpdateBiddingDetailsAsync(int id, BiddingDetailsUpdateModel model, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdWithDetailsAsync(id, ct);

            if (lot == null) return Result.NotFound();

            var active = lot.OpenDate < DateTime.Now;

            if (model.OpenDate != null && active)
                return Result.Error("Cannot set open date if lot is active");

            if (model.MinimalBid != lot.BiddingDetails.MinimalBid && active)
                return Result.Error("Cannot change minimal bid if lot is active");

            if ((model.OpenDate != null || model.CloseDate != null) && lot.BiddingDetails.Sold)
                return Result.Error("Cannot change bidding details when lot is sold");

            if (model.OpenDate != null) lot.OpenDate = (DateTime)model.OpenDate;
            if (model.CloseDate != null) lot.CloseDate = (DateTime)model.CloseDate;
            if (model.MinimalBid != lot.BiddingDetails.MinimalBid) lot.BiddingDetails.MinimalBid = model.MinimalBid;

            uof.LotRepository.Update(lot);
            await uof.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateStatusAsync(int lotId, int statusId, CancellationToken ct)
        {
            var lot = await uof.LotRepository.GetByIdAsync(lotId, ct);

            if (lot == null) return Result.NotFound();

            lot.StatusId = statusId;

            uof.LotRepository.Update(lot);
            await uof.SaveAsync(ct);

            return Result.Success();
        }
    }
}
