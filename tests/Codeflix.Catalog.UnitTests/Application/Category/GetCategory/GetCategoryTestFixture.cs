using Codeflix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTests.Application.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }

    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
