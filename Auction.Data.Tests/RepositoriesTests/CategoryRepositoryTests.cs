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
    public class CategoryRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ShouldReturnSingleCategory(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new CategoryRepository(context);
            var category = await repository.GetByIdAsync(id);

            var expected = ExpectedCategories.FirstOrDefault(b => b.Id == id);

            Assert.That(category,
                Is.EqualTo(expected).Using(new CategoryEqualityComparer()),
                message: "GetByIdAsync does not return expected category");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new CategoryRepository(context);
            var category = await repository.GetAllAsync();

            Assert.That(category,
                Is.EqualTo(ExpectedCategories).Using(new CategoryEqualityComparer()),
                message: "GetAllAsync does not return expected categories");
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ShouldReturnCategoryWithDetails(int id)
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new CategoryRepository(context);
            var category = await repository.GetByIdWithDetailsAsync(id);

            var expectedCategory = ExpectedCategories.FirstOrDefault(s => s.Id == id);

            Assert.That(category,
                Is.EqualTo(expectedCategory).Using(new CategoryEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected category");

            Assert.That(category.Lots,
                Is.EqualTo(ExpectedLots.Where(l => l.CategoryId == category.Id))
                .Using(new LotEqualityComparer()),
                message: "Category does not contain expected lots");
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ShouldReturnCategoriesWithDetails()
        {
            using var context = TestDbContextProvider.CreateContext();

            var repository = new CategoryRepository(context);
            var categories = await repository.GetAllWithDetailsAsync();

            Assert.That(categories,
                Is.EqualTo(ExpectedCategories).Using(new CategoryEqualityComparer()),
                message: "GetByIdWithDetailsAsync does not return expected category");

            Assert.That(categories.SelectMany(c => c.Lots).OrderBy(l => l.Id),
                Is.EqualTo(ExpectedLots).Using(new LotEqualityComparer()),
                message: "Category does not contain expected lots");
        }

        [Test]
        public async Task AddAsync_ShouldReturnExpectedCategoriesCount()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new CategoryRepository(context);

            var category = new Category { Name = "Test" };

            await repository.AddAsync(category);
            await context.SaveChangesAsync();

            Assert.That(context.Categories.Count(), Is.EqualTo(8),
                message: "AddAsync does not add new category to db");
        }

        [Test]
        public async Task DeleteByIdAsync_ShouldRemoveCategory()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new CategoryRepository(context);

            await repository.DeleteByIdAsync(2);
            await context.SaveChangesAsync();

            Assert.That(context.Categories.Count(), Is.EqualTo(6),
                message: "DeleteByIdAsync does not remove entity from db");
        }

        [Test]
        public async Task Delete_ShouldRemoveCategory()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new CategoryRepository(context);

            var category = new Category { Id = 1, Name = "Clothes" };

            repository.Delete(category);
            await context.SaveChangesAsync();

            Assert.That(context.Categories.Count(), Is.EqualTo(6),
                message: "Delete does not remove entity from db");
        }

        [Test]
        public async Task Update_ShouldUpdateCategory()
        {
            using var context = TestDbContextProvider.CreateContext();
            var repository = new CategoryRepository(context);

            var category = new Category { Id = 1, Name = "Test" };

            repository.Update(category);
            await context.SaveChangesAsync();

            Assert.That(category,
                Is.EqualTo(new Category { Id = 1, Name = "Test" })
                .Using(new CategoryEqualityComparer()),
                message: "Update does not modify entity");
        }

        private static readonly IEnumerable<Category> ExpectedCategories = new Category[]
        {
            new Category { Id = 1, Name = "Clothes" },
            new Category { Id = 2, Name = "Jewelry" },
            new Category { Id = 3, Name = "Art" },
            new Category { Id = 4, Name = "Games" },
            new Category { Id = 5, Name = "Sculpture" },
            new Category { Id = 6, Name = "Misc"},
            new Category { Id = 7, Name = "Books"}
        };

        private static readonly IEnumerable<Lot> ExpectedLots = new Lot[]
        {
            new Lot { Id = 1, Name = "Emerald ring", CategoryId = 2, SellerId = 1, StartPrice = 1200, StatusId = 1, OpenDate = new DateTime(2022, 6, 16), CloseDate = new DateTime(2022, 6, 26) },
            new Lot { Id = 2, Name = "Landscape painting", CategoryId = 3, SellerId = 1, BuyerId = 3, StartPrice = 500, StatusId = 2, OpenDate = new DateTime(2021, 1, 11), CloseDate = new DateTime(2021, 3, 11) },
            new Lot { Id = 3, Name = "Leather Jacket", CategoryId = 1, SellerId = 2, StartPrice = 200, StatusId = 3, OpenDate = new DateTime(2022, 1, 1), CloseDate = new DateTime(2022, 1, 11)},
            new Lot { Id = 4, Name = "War and Peace First Edition", CategoryId = 7, SellerId = 2, StatusId = 4, StartPrice = 350}
        };
    }
}
