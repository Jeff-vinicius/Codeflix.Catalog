using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture> { }

    public class CreateCategoryApiTestFixture : CategoryBaseFixture
    {
        public CreateCategoryInput GetExampleInput()
            => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }
}
