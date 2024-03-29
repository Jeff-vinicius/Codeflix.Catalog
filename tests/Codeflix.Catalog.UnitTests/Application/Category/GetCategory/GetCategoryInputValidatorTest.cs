﻿using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using Xunit;
using FluentAssertions;

namespace Codeflix.Catalog.UnitTests.Application.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryInputValidatorTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "GetCategoryInputValidator - Use Cases")]
        public void ValidationOk()
        {
            var validInput = new GetCategoryInput(Guid.NewGuid());
            var validator = new GetCategoryInputValidator();

            var validationResult = validator.Validate(validInput);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
        [Trait("Application", "GetCategoryInputValidator - Use Cases")]
        public void InvalidWhenEmptyGuidId()
        {
            var invalidInput = new GetCategoryInput(Guid.Empty);
            var validator = new GetCategoryInputValidator();

            var validationResult = validator.Validate(invalidInput);

            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().HaveCount(1);
            validationResult.Errors[0].ErrorMessage.Should().Be("'Id' deve ser informado.");
        }
    }
}
