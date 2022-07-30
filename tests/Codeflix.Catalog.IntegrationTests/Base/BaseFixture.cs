﻿using Bogus;

namespace Codeflix.Catalog.IntegrationTests.Base
{
    public class BaseFixture
    {
        public BaseFixture()
            => Faker = new Faker("pt_BR");

        protected Faker Faker { get; set; }
    }
}