using Moq;
using Xunit;
using FluentAssertions;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using Codeflix.Catalog.Application.Exceptions;

namespace Codeflix.Catalog.UnitTests.Application.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Application", "GetCategory - Use Cases")]
        public async Task GetCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exempleCategory = _fixture.GetExampleCategory();
            repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(exempleCategory);
            var input = new UseCase.GetCategoryInput(exempleCategory.Id);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            output.Id.Should().Be(exempleCategory.Id);
            output.Should().NotBeNull();
            output.Name.Should().Be(exempleCategory.Name);
            output.Description.Should().Be(exempleCategory.Description);
            output.IsActive.Should().Be(exempleCategory.IsActive);
            output.CreatedAt.Should().Be(exempleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Application", "GetCategory - Use Cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exempleGuid = Guid.NewGuid();
            repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException($"Category {exempleGuid} not found"));
            var input = new UseCase.GetCategoryInput(exempleGuid);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            var task = async ()
                => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
