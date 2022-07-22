using Xunit;
using FluentAssertions;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryInputValidatorTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
        [Trait("Application", "UpdateCategoryInputValidatorTest - Use Cases")]
        public void DontValidateWhenEmptyGuid()
        {
            var input = _fixture.GetValidInput(Guid.Empty);
            var validator = new UseCase.UpdateCategoryInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeFalse();
            validateResult.Errors.Should().HaveCount(1);
            validateResult.Errors[0].ErrorMessage.Should().Be("'Id' deve ser informado.");
        }

        [Fact(DisplayName = nameof(Validate))]
        [Trait("Application", "UpdateCategoryInputValidatorTest - Use Cases")]
        public void Validate()
        {
            var input = _fixture.GetValidInput();
            var validator = new UseCase.UpdateCategoryInputValidator();

            var validateResult = validator.Validate(input);

            validateResult.Should().NotBeNull();
            validateResult.IsValid.Should().BeTrue();
            validateResult.Errors.Should().HaveCount(0);
        }
    }
}
