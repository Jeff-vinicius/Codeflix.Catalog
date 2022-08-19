using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Infra.Data.EF;
using Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using AplicationUseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Microsoft.EntityFrameworkCore;
using Codeflix.Catalog.Application.Exceptions;

namespace Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(DomainEntity.Category exampleCategory, UpdateCategoryInput input)
        {
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new AplicationUseCase.UpdateCategory(repository, unitOfWork);


            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);

            var dbCategory = await dbContext.Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutIsActive(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new AplicationUseCase.UpdateCategory(repository, unitOfWork);


            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);

            var dbCategory = await dbContext.Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((exampleCategory.IsActive));
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryOnlyName(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new AplicationUseCase.UpdateCategory(repository, unitOfWork);


            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);

            var dbCategory = await dbContext.Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be((exampleCategory.IsActive));
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

        }

        [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        public async Task UpdateThrowsWhenNotFoundCategory()
        {
            var input = _fixture.GetValidInput();
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new AplicationUseCase.UpdateCategory(repository, unitOfWork);


            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category {input.Id} not found.");
        }
    }
}
