using Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using Codeflix.Catalog.IntegrationTests.Base;
using Xunit;

namespace Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }

    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {

    }
}
