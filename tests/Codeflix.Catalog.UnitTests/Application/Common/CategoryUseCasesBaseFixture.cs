﻿using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository;
using Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace Codeflix.Catalog.UnitTests.Application.Common
{
    public abstract class CategoryUseCasesBaseFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetRepositoryMock()
        => new();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();

        public string GetValidCategoryName()
        {
            var categoryName = string.Empty;

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName[..255];

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }

        public bool GetRandomBoolean()
            => (new Random()).NextDouble() < 0.5;

        public Category GetExampleCategory()
          => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }
}
