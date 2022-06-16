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
    public class BidRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSingleBid(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BidRepository(context);
            var bid = await repository.GetByIdAsync(id);

            var expected = ExpectedBids.FirstOrDefault(b => b.Id == id);

            Assert.That(bid,
                Is.EqualTo(expected).Using(new BidEqualityComparer()),
                message: "GetByIdAsync does not return expected bid");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllBids()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BidRepository(context);
            var bid = await repository.GetAllAsync();

            Assert.That(bid,
                Is.EqualTo(ExpectedBids).Using(new BidEqualityComparer()),
                message: "GetAllAsync does not return expected bid");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnBidWithDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BidRepository(context);
            var bid = await repository.GetByIdWithDetailsAsync(id);

            var expectedBid = ExpectedBids.FirstOrDefault(s => s.Id == id);
            var expectedDetails = ExpectedBiddingDetails.FirstOrDefault(d => d.Id == bid.BiddingDetailsId);
            var expectedBidder = ExpectedPersons.FirstOrDefault(p => p.Id == bid.BidderId);

            Assert.That(bid,
                Is.EqualTo(expectedBid).Using(new BidEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected bid");

            Assert.That(bid.BiddingDetails,
                Is.EqualTo(expectedDetails).Using(new BiddingDetailsEqualityComparer()),
                message: "Bid does not contain expected details");

            Assert.That(bid.Bidder,
                Is.EqualTo(expectedBidder).Using(new PersonEqualityComparer()),
                message: "Bid does not refer to expected bidder");
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ShouldReturnBidsWithDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new BidRepository(context);
            var bids = await repository.GetAllWithDetailsAsync();

            Assert.That(bids,
                Is.EqualTo(ExpectedBids).Using(new BidEqualityComparer()),
                message: "GetAllWithDetailsAsync does not return expected bids");

            Assert.That(bids.Select(b => b.BiddingDetails).Distinct(new BiddingDetailsEqualityComparer()).OrderBy(d => d.Id),
                Is.EqualTo(ExpectedBiddingDetails).Using(new BiddingDetailsEqualityComparer()),
                message: "Bids does not contain expected details");

            Assert.That(bids.Select(b => b.Bidder).OrderBy(b => b.Id),
                Is.EqualTo(ExpectedPersons).Using(new PersonEqualityComparer()),
                message: "Bids does not refer to expected bidders");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedBidsCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BidRepository(context);

            var bid = new Bid { BiddingDetailsId = 1, BidderId = 2, Price = 300, PlacedOn = DateTime.UtcNow };

            await repository.AddAsync(bid);
            await context.SaveChangesAsync();

            Assert.That(context.Bids.Count(), Is.EqualTo(3),
                message: "AddAsync does not add new bidding details to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveBid()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BidRepository(context);

            await repository.DeleteByIdAsync(2);
            await context.SaveChangesAsync();

            Assert.That(context.Bids.Count(), Is.EqualTo(1),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemoveBid()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BidRepository(context);

            var bid = new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 400 };

            repository.Delete(bid);
            await context.SaveChangesAsync();

            Assert.That(context.Bids.Count(), Is.EqualTo(1),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdateBid()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new BidRepository(context);

            var bid = new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 500 };

            repository.Update(bid);
            await context.SaveChangesAsync();

            Assert.That(bid,
                Is.EqualTo(new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 500 })
                .Using(new BidEqualityComparer()),
                message: "Update does not modify entity");
        }

        private static readonly IEnumerable<BiddingDetails> ExpectedBiddingDetails = new[]
        {
            new BiddingDetails { Id = 2, LotId = 2, MinimalBid = 100, CurrentBid = 1300, Sold = true }
        };

        private static readonly IEnumerable<Bid> ExpectedBids = new[]
        {
            new Bid { Id = 1, BiddingDetailsId = 2, BidderId = 2, PlacedOn = new DateTime(2021, 1, 2), Price = 400 },
            new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 400 }
        };

        private static readonly IEnumerable<Person> ExpectedPersons = new[]
        {
            new Person { Id = 2, Name = "Sara", Surname = "Parker", BirthDate = new DateTime(1972, 11, 2)},
            new Person { Id = 3, Name = "Hanna", Surname = "Winslow", BirthDate = new DateTime(1990, 1, 1)}
        };
    }
}
