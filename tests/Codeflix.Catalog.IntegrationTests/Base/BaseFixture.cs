using Bogus;
using Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Codeflix.Catalog.IntegrationTests.Base
{
    public class BaseFixture
    {
        public BaseFixture()
            => Faker = new Faker("pt_BR");

        protected Faker Faker { get; set; }

        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CodeflixCatalogDbContext(new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db").Options);
            if (!preserveData)
                context.Database.EnsureDeleted();
            return context;
        }
    }
}
