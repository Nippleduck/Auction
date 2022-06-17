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
    public class ReviewDetailsRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSingleReviewDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new ReviewDetailsRepository(context);
            var review = await repository.GetByIdAsync(id);

            var expected = ExpectedReviewDetails.FirstOrDefault(b => b.Id == id);

            Assert.That(review,
                Is.EqualTo(expected).Using(new ReviewDetailsEqualityComparer()),
                message: "GetByIdAsync does not return expected review");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllReviewDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new ReviewDetailsRepository(context);
            var review = await repository.GetAllAsync();

            Assert.That(review,
                Is.EqualTo(ExpectedReviewDetails).Using(new ReviewDetailsEqualityComparer()),
                message: "GetAllAsync does not return expected reviews");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnReviewDetailsWithInnerDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new ReviewDetailsRepository(context);
            var review = await repository.GetByIdWithDetailsAsync(id);

            var expectedReview = ExpectedReviewDetails.FirstOrDefault(s => s.Id == id);
            var expectedLot = ExpectedLots.FirstOrDefault(l => l.Id == review.LotId);

            Assert.That(review,
                Is.EqualTo(expectedReview).Using(new ReviewDetailsEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected review");

            Assert.That(review.Lot,
                Is.EqualTo(expectedLot).Using(new LotEqualityComparer()),
                message: "Review does not contain expected lots");
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ShouldReturnReviewDetailsWithInnerDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new ReviewDetailsRepository(context);
            var reviews = await repository.GetAllWithDetailsAsync();

            Assert.That(reviews,
                Is.EqualTo(ExpectedReviewDetails).Using(new ReviewDetailsEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected reviews");

            Assert.That(reviews.Select(r => r.Lot).OrderBy(b => b.Id),
                Is.EqualTo(ExpectedLots).Using(new LotEqualityComparer()),
                message: "Reviews does not contain expected lots");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedReviewDetailsCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new ReviewDetailsRepository(context);


            var review = new ReviewDetails { Status = ReviewStatus.PendingReview };

            await repository.AddAsync(review);
            await context.SaveChangesAsync();

            Assert.That(context.Reviews.Count(), Is.EqualTo(5),
                message: "AddAsync does not add new review to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveReviewDetails()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new ReviewDetailsRepository(context);

            await repository.DeleteByIdAsync(4);
            await context.SaveChangesAsync();

            Assert.That(context.Reviews.Count(), Is.EqualTo(3),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemoveReviewDetails()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new ReviewDetailsRepository(context);

            var review = new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.PendingReview };

            repository.Delete(review);
            await context.SaveChangesAsync();

            Assert.That(context.Reviews.Count(), Is.EqualTo(3),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdateReviewDetails()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new ReviewDetailsRepository(context);

            var review = new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.Allowed };

            repository.Update(review);
            await context.SaveChangesAsync();

            Assert.That(review,
                Is.EqualTo(new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.Allowed })
                .Using(new ReviewDetailsEqualityComparer()),
                message: "Update does not modify entity");
        }

        private static readonly IEnumerable<ReviewDetails> ExpectedReviewDetails = new[]
        {
            new ReviewDetails { Id = 1, LotId = 1, Status = ReviewStatus.Allowed },
            new ReviewDetails { Id = 2, LotId = 2, Status = ReviewStatus.Allowed },
            new ReviewDetails { Id = 3, LotId = 3, Status = ReviewStatus.Allowed },
            new ReviewDetails { Id = 4, LotId = 4, Status = ReviewStatus.PendingReview}
        };

        private static readonly IEnumerable<Lot> ExpectedLots = new[]
        {
            new Lot { Id = 1, Name = "Emerald ring", CategoryId = 2, SellerId = 1, StartPrice = 1200, StatusId = 1, OpenDate = new DateTime(2022, 6, 16), CloseDate = new DateTime(2022, 6, 26) },
            new Lot { Id = 2, Name = "Landscape painting", CategoryId = 3, SellerId = 1, BuyerId = 3, StartPrice = 500, StatusId = 2, OpenDate = new DateTime(2021, 1, 11), CloseDate = new DateTime(2021, 3, 11) },
            new Lot { Id = 3, Name = "Leather Jacket", CategoryId = 1, SellerId = 2, StartPrice = 200, StatusId = 3, OpenDate = new DateTime(2022, 1, 1), CloseDate = new DateTime(2022, 1, 11)},
            new Lot { Id = 4, Name = "War and Peace First Edition", CategoryId = 7, SellerId = 2, StatusId = 4, StartPrice = 350}
        };
    }
}
