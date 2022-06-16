using Auction.Data.Repositories;
using Auction.Domain.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Data.Tests.RepositoriesTests
{
    [TestFixture]
    public class BiddingDetailsRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSingleBiddingDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BiddingDetailsRepository(context);
            var biddingDetails = await repository.GetByIdAsync(id);

            var expected = ExpectedBiddingDetails.FirstOrDefault(b => b.Id == id);

            Assert.That(biddingDetails,
                Is.EqualTo(expected).Using(new BiddingDetailsEqualityComparer()),
                message: "GetByIdAsync does not return expected bidding details");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllBiddingDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BiddingDetailsRepository(context);
            var biddingDetails = await repository.GetAllAsync();

            Assert.That(biddingDetails,
                Is.EqualTo(ExpectedBiddingDetails).Using(new BiddingDetailsEqualityComparer()),
                message: "GetAllAsync does not return expected bidding details");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnBiddingDetailsWithInnerDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BiddingDetailsRepository(context);
            var biddingDetails = await repository.GetByIdWithDetailsAsync(id);

            var expectedDetails = ExpectedBiddingDetails.FirstOrDefault(s => s.Id == id);
            var expectedLot = ExpectedLots.FirstOrDefault(l => l.Id == biddingDetails.Id);

            Assert.That(biddingDetails,
                Is.EqualTo(expectedDetails).Using(new BiddingDetailsEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected details");

            Assert.That(biddingDetails.Lot,
                Is.EqualTo(expectedLot).Using(new LotEqualityComparer()),
                message: "Bidding details does not contain to expected lot");

            Assert.That(biddingDetails.Bids,
                Is.EqualTo(ExpectedBids.Where(b => b.BiddingDetailsId == biddingDetails.Id))
                .Using(new BidEqualityComparer()),
                message: "Bidding details does not contain to expected bids");
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ShouldReturnBiddingDetailsWithInnerDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BiddingDetailsRepository(context);
            var biddingDetails = await repository.GetAllWithDetailsAsync();

            Assert.That(biddingDetails,
                Is.EqualTo(ExpectedBiddingDetails).Using(new BiddingDetailsEqualityComparer()),
                message: "GetAllWithDetailsAsync does not return expected details");

            Assert.That(biddingDetails.Select(s => s.Lot).OrderBy(l => l.Id),
                Is.EqualTo(ExpectedLots).Using(new LotEqualityComparer()),
                message: "Statuses does not refer to expected lots");

            Assert.That(biddingDetails.SelectMany(bd => bd.Bids).OrderBy(b => b.Id),
                Is.EqualTo(ExpectedBids).Using(new BidEqualityComparer()),
                message: "Bidding details does not contain to expected bids");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedBiddingDetailsCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BiddingDetailsRepository(context);

            var biddingDetails = new BiddingDetails { MinimalBid = 100, CurrentBid = 300 };

            await repository.AddAsync(biddingDetails);
            await context.SaveChangesAsync();

            Assert.That(context.BiddingDetails.Count(), Is.EqualTo(4),
                message: "AddAsync does not add new bidding details to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveBiddingDetails()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BiddingDetailsRepository(context);

            await repository.DeleteByIdAsync(2);
            await context.SaveChangesAsync();

            Assert.That(context.BiddingDetails.Count(), Is.EqualTo(2),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemoveBiddingDetails()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BiddingDetailsRepository(context);

            var details = new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 1300, Sold = true };

            repository.Delete(details);
            await context.SaveChangesAsync();

            Assert.That(context.BiddingDetails.Count(), Is.EqualTo(2),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdateBiddingDetails()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BiddingDetailsRepository(context);

            var details = new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 2000, Sold = true };

            repository.Update(details);
            await context.SaveChangesAsync();

            Assert.That(details,
                Is.EqualTo(new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 2000, Sold = true })
                .Using(new BiddingDetailsEqualityComparer()),
                message: "Update does not modify entity");
        }

        private static readonly IEnumerable<BiddingDetails> ExpectedBiddingDetails = new []
        {
            new BiddingDetails { Id = 1, LotId = 1, MinimalBid = 200, CurrentBid = 1200 },
            new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 1300, Sold = true },
            new BiddingDetails { Id = 3, LotId = 3, MinimalBid = 50, CurrentBid = 200 }
        };

        private static readonly IEnumerable<Lot> ExpectedLots = new[]
        {
            new Lot {
                Id = 1,
                Name = "Emerald ring",
                CategoryId = 2,
                SellerId = 1,
                StartPrice = 1200,
                StatusId = 1,
                OpenDate = new DateTime(2022, 6, 16),
                CloseDate = new DateTime(2022, 6, 26) },
            new Lot {
                Id = 2,
                Name = "Landscape painting",
                CategoryId = 3,
                SellerId = 1,
                BuyerId = 3,
                StartPrice = 500,
                StatusId = 2,
                OpenDate = new DateTime(2021, 1, 11),
                CloseDate = new DateTime(2021, 3, 11) },
            new Lot { Id = 3,
                Name = "Leather Jacket",
                CategoryId = 1,
                SellerId = 2,
                StartPrice = 200,
                StatusId = 3,
                OpenDate = new DateTime(2022, 1, 1),
                CloseDate = new DateTime(2022, 1, 11)}
        };

        private static readonly IEnumerable<Bid> ExpectedBids = new[]
        {
            new Bid { Id = 1, BiddingDetailsId = 2, BidderId = 2, PlacedOn = new DateTime(2021, 1, 2), Price = 400 },
            new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 400 }

        };
    }
}
