using Codeflix.Catalog.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeflix.Catalog.EndToEndTests.Base
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbOptions = services.FirstOrDefault(
                    x => x.ServiceType == typeof(
                    DbContextOptions<CodeflixCatalogDbContext>)
                    );
                if (dbOptions is not null)
                    services.Remove(dbOptions);
                services.AddDbContext<CodeflixCatalogDbContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase("end2end-tests-db");
                    });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
