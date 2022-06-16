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
    public class PersonRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSinglePerson(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new PersonRepository(context);
            var person = await repository.GetByIdAsync(id);

            var expected = ExpectedPersons.FirstOrDefault(b => b.Id == id);

            Assert.That(person,
                Is.EqualTo(expected).Using(new PersonEqualityComparer()),
                message: "GetByIdAsync does not return expected person");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllPersons()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new PersonRepository(context);
            var persons = await repository.GetAllAsync();

            Assert.That(persons,
                Is.EqualTo(ExpectedPersons).Using(new PersonEqualityComparer()),
                message: "GetAllAsync does not return expected persons");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnPersonWithDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new PersonRepository(context);
            var person = await repository.GetByIdWithDetailsAsync(id);

            var expectedPerson = ExpectedPersons.FirstOrDefault(s => s.Id == id);

            Assert.That(person,
                Is.EqualTo(expectedPerson).Using(new PersonEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected person");

            Assert.That(person.Bids,
                Is.EqualTo(ExpectedBids.Where(b => b.BidderId == person.Id))
                .Using(new BidEqualityComparer()),
                message: "Person does not contain expected bids");

            Assert.That(person.PurchasedLots,
                Is.EqualTo(ExpectedLots.Where(l => l.BuyerId == person.Id))
                .Using(new LotEqualityComparer()),
                message: "Person does not contain expected purchased lots");

            Assert.That(person.OwnedLots,
                Is.EqualTo(ExpectedLots.Where(l => l.SellerId == person.Id))
                .Using(new LotEqualityComparer()),
                message: "Person does not contain expected selling lots");
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ShouldReturnPersonsWithDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new PersonRepository(context);
            var persons = await repository.GetAllWithDetailsAsync();

            Assert.That(persons,
                Is.EqualTo(ExpectedPersons).Using(new PersonEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected persons");

            Assert.That(persons.SelectMany(p => p.Bids).OrderBy(b => b.Id),
                Is.EqualTo(ExpectedBids).Using(new BidEqualityComparer()),
                message: "Persons does not contain expected bids");

            Assert.That(persons.SelectMany(p => p.PurchasedLots).OrderBy(l => l.Id),
                Is.EqualTo(ExpectedLots.Where(l => l.BuyerId != 0))
                .Using(new LotEqualityComparer()),
                message: "Persons does not contain expected purchased lots");

            Assert.That(persons.SelectMany(p => p.OwnedLots).OrderBy(l => l.Id),
                Is.EqualTo(ExpectedLots).Using(new LotEqualityComparer()),
                message: "Persons does not contain expected selling lots");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedPersonsCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new PersonRepository(context);

            var person = new Person { Name = "Test", Surname = "Test", BirthDate = DateTime.UtcNow };

            await repository.AddAsync(person);
            await context.SaveChangesAsync();

            Assert.That(context.Persons.Count(), Is.EqualTo(4),
                message: "AddAsync does not add new person to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemovePerson()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new PersonRepository(context);

            await repository.DeleteByIdAsync(2);
            await context.SaveChangesAsync();

            Assert.That(context.Persons.Count(), Is.EqualTo(2),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemovePerson()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new PersonRepository(context);

            var person = new Person { Id = 1, Name = "James", Surname = "Longhart", BirthDate = new DateTime(1997, 4, 3) };

            repository.Delete(person);
            await context.SaveChangesAsync();

            Assert.That(context.Persons.Count(), Is.EqualTo(2),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdatePerson()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new PersonRepository(context);

            var person = new Person { Id = 1, Name = "James", Surname = "Shorthart", BirthDate = new DateTime(1997, 4, 3) };

            repository.Update(person);
            await context.SaveChangesAsync();

            Assert.That(person,
                Is.EqualTo(new Person { Id = 1, Name = "James", Surname = "Shorthart", BirthDate = new DateTime(1997, 4, 3) })
                .Using(new PersonEqualityComparer()),
                message: "Update does not modify entity");
        }

        private static readonly IEnumerable<Person> ExpectedPersons = new[]
        {
            new Person { Id = 1, Name = "James", Surname = "Longhart", BirthDate = new DateTime(1997, 4, 3)},
            new Person { Id = 2, Name = "Sara", Surname = "Parker", BirthDate = new DateTime(1972, 11, 2)},
            new Person { Id = 3, Name = "Hanna", Surname = "Winslow", BirthDate = new DateTime(1990, 1, 1)}
        };

        private static readonly IEnumerable<Bid> ExpectedBids = new[]
        {
             new Bid { Id = 1, BiddingDetailsId = 2, BidderId = 2, PlacedOn = new DateTime(2021, 1, 2), Price = 400 },
             new Bid { Id = 2, BiddingDetailsId = 2, BidderId = 3, PlacedOn = new DateTime(2021, 1, 3), Price = 400 }
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
