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
    internal class AuctionStatusRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSingleStatus(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var statusRepository = new AuctionStatusRepository(context);
            var status = await statusRepository.GetByIdAsync(id);

            var expected = ExpectedStatuses.FirstOrDefault(s => s.Id == id);

            Assert.That(status,
                Is.EqualTo(expected).Using(new AuctionStatusEqualityComparer()),
                message: "GetByIdAsync does not return expected status");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllStatuses()
        {
            using var context = TestDbContextProvider.CreateContext();

            var statusRepository = new AuctionStatusRepository(context);
            var statuses = await statusRepository.GetAllAsync();

            Assert.That(statuses,
                Is.EqualTo(ExpectedStatuses).Using(new AuctionStatusEqualityComparer()),
                message: "GetAllAsync does not return expected statuses");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnStatusWithLotsDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var statusRepository = new AuctionStatusRepository(context);
            var status = await statusRepository.GetByIdWithDetailsAsync(id);

            var expectedStatus = ExpectedStatuses.FirstOrDefault(s => s.Id == id);
            var expectedLot = ExpectedLots.FirstOrDefault(l => l.StatusId == id);

            Assert.That(status,
                Is.EqualTo(expectedStatus).Using(new AuctionStatusEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected status");

            Assert.That(status.Lots.First(),
                Is.EqualTo(expectedLot).Using(new LotEqualityComparer()),
                message: "Status does not refer to expected lots");
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ShouldReturnStatusesWithLots()
        {
            using var context = TestDbContextProvider.CreateContext();

            var statusRepository = new AuctionStatusRepository(context);
            var statuses = await statusRepository.GetAllWithDetailsAsync();

            Assert.That(statuses,
                Is.EqualTo(ExpectedStatuses).Using(new AuctionStatusEqualityComparer()),
                message: "GetAllWithDetailsAsync does not return expected statuses");

            Assert.That(statuses.SelectMany(s => s.Lots).OrderBy(l => l.Id),
                Is.EqualTo(ExpectedLots).Using(new LotEqualityComparer()),
                message: "Statuses does not refer to expected lots");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedStatusesCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var statusRepository = new AuctionStatusRepository(context);

            var status = new AuctionStatus { Name = "Test" };

            await statusRepository.AddAsync(status);
            await context.SaveChangesAsync();

            Assert.That(context.Statuses.Count(), Is.EqualTo(5), 
                message: "AddAsync does not add new status to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveStatus()
        {
            using var context = TestDbContextProvider.CreateContext();
            var statusRepository = new AuctionStatusRepository(context);

            await statusRepository.DeleteByIdAsync(4);
            await context.SaveChangesAsync();

            Assert.That(context.Statuses.Count(), Is.EqualTo(3),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemoveStatus()
        {
            using var context = TestDbContextProvider.CreateContext();
            var statusRepository = new AuctionStatusRepository(context);

            var status = new AuctionStatus { Id = 4, Name = "Not Started" };

            statusRepository.Delete(status);
            await context.SaveChangesAsync();

            Assert.That(context.Statuses.Count(), Is.EqualTo(3),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdateEntity()
        {
            using var context = TestDbContextProvider.CreateContext();
            var statusRepository = new AuctionStatusRepository(context);

            var status = new AuctionStatus { Id = 1, Name = "TestUpdated" };

            statusRepository.Update(status);
            await context.SaveChangesAsync();

            Assert.That(status, 
                Is.EqualTo( new AuctionStatus { Id = 1, Name = "TestUpdated" })
                .Using(new AuctionStatusEqualityComparer()),
                message: "Update does not modify entity");
        }

        private static readonly IEnumerable<AuctionStatus> ExpectedStatuses = new[]
        {
            new AuctionStatus { Id = 1, Name = "Auctioning" },
            new AuctionStatus { Id = 2, Name = "Lot Sold" },
            new AuctionStatus { Id = 3, Name = "Over" },
            new AuctionStatus { Id = 4, Name = "Not Started" }
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
                CloseDate = new DateTime(2022, 1, 11)},
            new Lot { Id = 4,
                Name = "War and Peace First Edition", 
                CategoryId = 7, 
                SellerId = 2, 
                StatusId = 4, 
                StartPrice = 350}

        };
    }
}
