using Bogus;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            var value = Faker.Commerce.ProductName();

            Action action = 
                () =>  DomainValidation.NotNull(value, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNull(value, fieldName);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should not be null");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            var value = Faker.Commerce.ProductName();
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNullOrEmpty(value, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNullOrEmpty(target, fieldName);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should not be empty or null");
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanTheMin), parameters: 6)]
        public void MinLengthOk(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesGreaterThanTheMin(int numberOfTests = 6)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                var valueExample = faker.Commerce.ProductName();
                var minLength = valueExample.Length - (new Random()).Next(1, 5);
                yield return new object[]
                {
                   valueExample, minLength
                };
            }
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanTheMin), parameters: 6)]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be at leats {minLength} characteres long");
        }

        public static IEnumerable<object[]> GetValuesSmallerThanTheMin(int numberOfTests = 6)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                var valueExample = faker.Commerce.ProductName();
                var minLength = valueExample.Length + (new Random()).Next(1, 20);
                yield return new object[]
                {
                   valueExample, minLength
                };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 6)]
        public void MaxLengthOk(string target, int maxLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 6)
        {
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var valueExample = faker.Commerce.ProductName();
                var maxLength = valueExample.Length + (new Random()).Next(0, 5);
                yield return new object[]
                {
                   valueExample, maxLength
                };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 6)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MaxLength(target, maxLength, fieldName);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 6)
        {
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var valueExample = faker.Commerce.ProductName();
                var maxLength = valueExample.Length - (new Random()).Next(1, 5);
                yield return new object[]
                {
                   valueExample, maxLength
                };
            }
        }
    }
}
