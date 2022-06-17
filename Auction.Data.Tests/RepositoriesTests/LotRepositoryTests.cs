using Auction.Data.Repositories;
using Auction.Domain.Entities;
using Auction.Domain.Entities.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Data.Tests.RepositoriesTests
{
    [TestFixture]
    public class LotRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSingleLot(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new LotRepository(context);
            var lot = await repository.GetByIdAsync(id);

            var expected = ExpectedLots.FirstOrDefault(b => b.Id == id);

            Assert.That(lot,
                Is.EqualTo(expected).Using(new LotEqualityComparer()),
                message: "GetByIdAsync does not return expected lot");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllLots()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new LotRepository(context);
            var lots = await repository.GetAllAsync();

            Assert.That(lots,
                Is.EqualTo(ExpectedLots).Using(new LotEqualityComparer()),
                message: "GetAllAsync does not return expected lots");
        }

        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnLotDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new LotRepository(context);
            var lot = await repository.GetByIdWithDetailsAsync(id);

            var expectedLot = ExpectedLots.FirstOrDefault(l => l.Id == lot.Id);
            var expectedReview = ExpectedReviewDetails.FirstOrDefault(r => r.LotId == lot.Id);
            var expectedBiddingDetails = ExpectedBiddingDetails.FirstOrDefault(b => b.LotId == lot.Id);
            var expectedCategory = ExpectedCategories.FirstOrDefault(c => c.Id == lot.CategoryId);
            var expectedStatus = ExpectedStatuses.FirstOrDefault(s => s.Id == lot.StatusId);
            var expectedSeller = ExpectedPersons.FirstOrDefault(p => p.Id == lot.SellerId);
            var expectedBuyer = ExpectedPersons.FirstOrDefault(p => p.Id == lot.BuyerId);

            Assert.That(lot,
                Is.EqualTo(expectedLot).Using(new LotEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected lot");

            Assert.That(lot.ReviewDetails,
                Is.EqualTo(expectedReview).Using(new ReviewDetailsEqualityComparer()),
                message: "Lot does not contain expected review details");

            Assert.That(lot.BiddingDetails,
                Is.EqualTo(expectedBiddingDetails).Using(new BiddingDetailsEqualityComparer()),
                message: "Lot does not contain expected bidding details");

            Assert.That(lot.Category,
                Is.EqualTo(expectedCategory).Using(new CategoryEqualityComparer()),
                message: "Lot does not contain expected category");

            Assert.That(lot.Status,
                Is.EqualTo(expectedStatus).Using(new AuctionStatusEqualityComparer()),
                message: "Lot does not contain expected status");

            Assert.That(lot.Seller,
                Is.EqualTo(expectedSeller).Using(new PersonEqualityComparer()),
                message: "Lot does not contain expected seller");

            Assert.That(lot.Buyer,
                Is.EqualTo(expectedBuyer).Using(new PersonEqualityComparer()),
                message: "Lot does not contain expected buyer");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedLotsCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new LotRepository(context);


            var lot = new Lot { Name = "Test", CategoryId = 2, StartPrice = 100 };

            await repository.AddAsync(lot);
            await context.SaveChangesAsync();

            Assert.That(context.Lots.Count(), Is.EqualTo(5),
                message: "AddAsync does not add new lot to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveLot()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new LotRepository(context);

            await repository.DeleteByIdAsync(4);
            await context.SaveChangesAsync();

            Assert.That(context.Lots.Count(), Is.EqualTo(3),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemoveLot()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new LotRepository(context);

            var lot = new Lot 
            { 
                Id = 4,
                Name = "War and Peace First Edition",
                CategoryId = 7, 
                SellerId = 2,
                StatusId = 4, 
                StartPrice = 350 };

            repository.Delete(lot);
            await context.SaveChangesAsync();

            Assert.That(context.Lots.Count(), Is.EqualTo(3),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdateLot()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new LotRepository(context);

            var lot = new Lot
            {
                Id = 4,
                Name = "War and Peace Second Edition",
                CategoryId = 7,
                SellerId = 2,
                StatusId = 4,
                StartPrice = 350
            };

            repository.Update(lot);
            await context.SaveChangesAsync();

            Assert.That(lot,
                Is.EqualTo(new Lot { Id = 4, Name = "War and Peace Second Edition", CategoryId = 7, SellerId = 2, StatusId = 4, StartPrice = 350 })
                .Using(new LotEqualityComparer()),
                message: "Update does not modify entity");
        }

        [TestCase("Emerald ring")]
        [TestCase("Landscape painting")]
        public async Task GetByNameAsync_ShouldReturnLotWithSpecifiedName(string name)
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new LotRepository(context);

            var lot = await repository.GetByNameAsync(name);

            Assert.That(lot,
                Is.EqualTo(ExpectedLots.FirstOrDefault(l => l.Name == name))
                .Using(new LotEqualityComparer()),
                message: "Method does not return expected lot with such name");
        }

        [Test]
        public async Task GetAllAvailableForSaleAsync_ShouldReturnAllAvailableLots()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new LotRepository(context);

            var lots = await repository.GetAllAvailableForSaleAsync();
            var expectedLots = ExpectedLots.Where(l => l.CloseDate != DateTime.MinValue && DateTime.UtcNow < l.CloseDate);

            Assert.That(lots,
                Is.EqualTo(expectedLots).Using(new LotEqualityComparer()),
                message: "Method does not return expected lots");

            Assert.That(lots.Select(l => l.Status).OrderBy(s => s.Id),
                Is.EqualTo(ExpectedStatuses.Join(expectedLots, s => s.Id, l => l.StatusId, (s, l) => s))
                .Using(new AuctionStatusEqualityComparer()),
                message: "Lots does not contain expected statuses");

            Assert.That(lots.Select(l => l.Category).OrderBy(c => c.Id),
                Is.EqualTo(ExpectedCategories.Join(expectedLots, c => c.Id, l => l.CategoryId, (c, l) => c))
                .Using(new CategoryEqualityComparer()),
                message: "Lots does not contain expected categorioes");
        }

        private static readonly IEnumerable<AuctionStatus> ExpectedStatuses = new[]
        {
            new AuctionStatus { Id = 1, Name = "Auctioning" },
            new AuctionStatus { Id = 2, Name = "Lot Sold" },
            new AuctionStatus { Id = 3, Name = "Over" },
            new AuctionStatus { Id = 4, Name = "Not Started" }
        };

        private static readonly IEnumerable<Category> ExpectedCategories = new[]
        {
            new Category { Id = 1, Name = "Clothes" },
            new Category { Id = 2, Name = "Jewelry" },
            new Category { Id = 3, Name = "Art" },
            new Category { Id = 7, Name = "Books" }
        };

        private static readonly IEnumerable<Person> ExpectedPersons = new[]
        {
            new Person { Id = 1, Name = "James", Surname = "Longhart", BirthDate = new DateTime(1997, 4, 3)},
            new Person { Id = 2, Name = "Sara", Surname = "Parker", BirthDate = new DateTime(1972, 11, 2)},
            new Person { Id = 3, Name = "Hanna", Surname = "Winslow", BirthDate = new DateTime(1990, 1, 1)}
        };

        private static readonly IEnumerable<Lot> ExpectedLots = new[]
        {
            new Lot { Id = 1, Name = "Emerald ring", CategoryId = 2, SellerId = 1, StartPrice = 1200, StatusId = 1, OpenDate = new DateTime(2022, 6, 16), CloseDate = new DateTime(2022, 6, 26) },
            new Lot { Id = 2, Name = "Landscape painting", CategoryId = 3, SellerId = 1, BuyerId = 3, StartPrice = 500, StatusId = 2, OpenDate = new DateTime(2021, 1, 11), CloseDate = new DateTime(2021, 3, 11) },
            new Lot { Id = 3, Name = "Leather Jacket", CategoryId = 1, SellerId = 2, StartPrice = 200, StatusId = 3, OpenDate = new DateTime(2022, 1, 1), CloseDate = new DateTime(2022, 1, 11)},
            new Lot { Id = 4, Name = "War and Peace First Edition", CategoryId = 7, SellerId = 2, StatusId = 4, StartPrice = 350}
        };

        private static readonly IEnumerable<BiddingDetails> ExpectedBiddingDetails = new[]
        {
            new BiddingDetails { Id = 1, LotId = 1, MinimalBid = 200, CurrentBid = 1200 },
            new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 1300, Sold = true },
            new BiddingDetails { Id = 3, LotId = 3, MinimalBid = 50, CurrentBid = 200 }
        };

        private static readonly IEnumerable<ReviewDetails> ExpectedReviewDetails = new[]
        {
            new ReviewDetails { Id = 1, LotId = 1, Status = ReviewStatus.Allowed },
            new ReviewDetails { Id = 2, LotId = 2, Status = ReviewStatus.Allowed },
            new ReviewDetails { Id = 3, LotId = 3, Status = ReviewStatus.Allowed },
            new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.PendingReview}
        };
    }
}
