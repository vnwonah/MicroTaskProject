using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MT_NetCore_Data.CatalogDB
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
               .Build();


            var builder = new DbContextOptionsBuilder<CatalogDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("CatalogDbConnection"));

            return new CatalogDbContext(builder.Options);
        }
    }
}
