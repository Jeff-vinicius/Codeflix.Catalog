using Xunit;
using FluentAssertions;
using Codeflix.Catalog.Infra.Data.EF;
using Repository = Codeflix.Catalog.Infra.Data.EF.Repositories;
using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [Collection(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTest
    {
        private readonly CategoryRepositoryTestFixture _fixture;

        public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Insert()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Insert(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Get()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(GetThrowIfNotFound))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category {exampleId} not found.");
           
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Update()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var newCategoryValues = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
            await categoryRepository.Update(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Delete()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Delete(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().BeNull();
        }

        [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnsListAndTotal()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);
            foreach (Category outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SearchReturnsPaginated))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(7, 3, 5, 0)]
        public async Task SearchReturnsPaginated(int quantityCategoriesToGenerate, int page, int perPage, int expectedQuantityItems )
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(quantityCategoriesToGenerate);
            output.Items.Should().HaveCount(expectedQuantityItems);
            foreach (Category outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 2, 2)]
        [InlineData("Horror", 2, 5, 0, 2)]
        public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(new List<string>()
            {
                "Action",
                "Horror",
                "Horror - Real Facts",
                "Drama",
                "Comedy"
            });
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);
            foreach (Category outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchOrdened))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]
        [InlineData("", "desc")]
        public async Task SearchOrdened(string orderBy, string order)
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(exampleCategoriesList, orderBy, searchOrder);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);
            for (int i = 0; i < exampleCategoriesList.Count; i++)
            {
                var expectedItem = expectedOrderedList[i];
                var outputItem = output.Items[i];
                expectedItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().Be(expectedItem.Id);
                outputItem.Name.Should().Be(expectedItem!.Name);
                outputItem.Description.Should().Be(expectedItem.Description);
                outputItem.IsActive.Should().Be(expectedItem.IsActive);
                outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
            }
        }
    }
}
