using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange 
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var dateTimeBefore = DateTime.Now;

            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var dateTimeAfter = DateTime.Now;

            //Assert 
            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);

            category.Id.Should().NotBeEmpty();

            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > dateTimeBefore).Should().BeTrue();
            (category.CreatedAt < dateTimeAfter).Should().BeTrue();

            (category.IsActive).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)] 
        public void InstantiateWithIsActive(bool isActive)
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };
            var dateTimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var dateTimeAfter = DateTime.Now;

            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);

            category.Id.Should().NotBeEmpty();

            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > dateTimeBefore).Should().BeTrue();
            (category.CreatedAt < dateTimeAfter).Should().BeTrue();

            (category.IsActive).Should().Be(isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action =
                () => new DomainEntity.Category(name!, "category description");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action =
                () => new DomainEntity.Category("category name", null!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Description should not be empty or null");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLess3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ab")]
        public void InstantiateErrorWhenNameIsLess3Characters(string invalidName)
        {
            Action action =
                () => new DomainEntity.Category(invalidName, "category description");

            action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be at leats 3 Characters");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToList());

            Action action =
                () => new DomainEntity.Category(invalidName, "category description");

            action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToList());

            Action action =
                () => new DomainEntity.Category("category name", invalidDescription);

            action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should be less or equal 10_000 characters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);
            category.Activate();

            category.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            category.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("category name", "category description");
            var newValues = new { Name = "new name", Description = "new description" };

            category.Update(newValues.Name, newValues.Description);

            category.Name.Should().Be(newValues.Name);
            category.Description.Should().Be(newValues.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("category name", "category description");
            var newValues = new { Name = "new name" };
            var currentDescription = category.Description;

            category.Update(newValues.Name);

            category.Name.Should().Be(newValues.Name);
            category.Description.Should().Be(currentDescription);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("category name", "category description");

            Action action =
                () => category.Update(name!);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLess3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ab")]
        public void UpdateErrorWhenNameIsLess3Characters(string invalidName)
        {
            var category = new DomainEntity.Category("category name", "category description");

            Action action =
                () => category.Update(invalidName!);

            action.Should().Throw<EntityValidationException>()
              .WithMessage("Name should be at leats 3 Characters");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = new DomainEntity.Category("category name", "category description");
            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToList());

            Action action =
               () => category.Update(invalidName);

            action.Should().Throw<EntityValidationException>()
              .WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var category = new DomainEntity.Category("category name", "category description");
            var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToList());

            Action action =
                () => category.Update("category name", invalidDescription);

            action.Should().Throw<EntityValidationException>()
              .WithMessage("Description should be less or equal 10_000 characters long");
        }
    }
}
