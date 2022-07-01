using Moq;
using Xunit;
using FluentAssertions;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _createCategoryTestFixture;

        public CreateCategoryTest(CreateCategoryTestFixture createCategoryTestFixture)
        {
            _createCategoryTestFixture = createCategoryTestFixture;
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();
            var repositoryMock = _createCategoryTestFixture.GetRepositoryMock();
          
            var useCase = new UseCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

            var input = _createCategoryTestFixture.GetInput();

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()),
                Times.Once);

            output.Id.Should().NotBeEmpty();
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        }
    }
}
