using Moq;
using Xunit;
using FluentAssertions;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Exceptions;

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

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyName()
        {
            var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();
            var repositoryMock = _createCategoryTestFixture.GetRepositoryMock();

            var useCase = new UseCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

            var input = new CreateCategoryInput(_createCategoryTestFixture.GetValidCategoryName());

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
            output.Description.Should().Be("");
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();
            var repositoryMock = _createCategoryTestFixture.GetRepositoryMock();

            var useCase = new UseCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

            var input = new CreateCategoryInput(_createCategoryTestFixture.GetValidCategoryName(),
                _createCategoryTestFixture.GetValidCategoryDescription());

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
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(CreateCategoryTestDataGenerator.GetInValidInputs),
            parameters: 12,
            MemberType = typeof(CreateCategoryTestDataGenerator))]
        public async void ThrowWhenCantInstantiateCategory(CreateCategoryInput createCategoryInput, string exceptionMessage)
        {
            var useCase = new UseCases.CreateCategory(
                _createCategoryTestFixture.GetUnitOfWorkMock().Object,
                _createCategoryTestFixture.GetRepositoryMock().Object);

            Func<Task> task = async () => await useCase.Handle(createCategoryInput, CancellationToken.None);

            await task.Should()
                .ThrowAsync<EntityValidationException>()
                .WithMessage(exceptionMessage);
        }
    }
}
