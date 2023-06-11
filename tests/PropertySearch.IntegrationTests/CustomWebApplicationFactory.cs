using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PropertySearchApp.Persistence;
using System.Data.Common;

namespace PropertySearch.IntegrationTests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextDescriptor is null)
                throw new InvalidOperationException("DbContextOptions<ApplicationDbContext> not found");

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));
            
            if(dbConnectionDescriptor is null)
                throw new InvalidOperationException("DbConnection not found");

            services.Remove(dbConnectionDescriptor);

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                options.UseInMemoryDatabase("test-database");
            });
        });

        builder.UseEnvironment("Development");
    }
}
