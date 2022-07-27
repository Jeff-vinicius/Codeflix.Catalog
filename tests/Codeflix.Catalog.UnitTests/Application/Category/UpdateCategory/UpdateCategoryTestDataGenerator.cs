﻿using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    public class UpdateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
        {
            var fixture = new UpdateCategoryTestFixture();
            for (int indice = 0; indice < times; indice++)
            {
                var exampleCategory = fixture.GetExampleCategory();
                var exampleInput = fixture.GetValidInput(exampleCategory.Id);

                yield return new object[] { exampleCategory, exampleInput };
            }
        }

        public static IEnumerable<object[]> GetInValidInputs(int times = 12)
        {
            var fixture = new UpdateCategoryTestFixture();
            var invalidInputList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputList.Add(new object[]
                        {
                            fixture.GetInvalidInputShortName(),
                            "Name should be at leats 3 characteres long"
                        });
                        break;
                    case 1:
                        invalidInputList.Add(new object[]
                        {
                            fixture.GetInvalidInputTooLongDescription(),
                            "Description should be less or equal 10000 characters long"
                        });
                        break;
                    default:
                        break;
                }
            }

            return invalidInputList;
        }
    }
}
