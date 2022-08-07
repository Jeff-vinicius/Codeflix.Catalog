using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }
    public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public CreateCategoryInput GetInput()
        {
            var category = GetExampleCategory();
            return new CreateCategoryInput(category.Name, category.Description, category.IsActive);
        }
    }
}
