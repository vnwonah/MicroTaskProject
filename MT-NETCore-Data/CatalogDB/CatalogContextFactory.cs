using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MT_NetCore_Data.CatalogDB
{
    public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CatalogDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("CatalogDbConnection"));

            return new CatalogDbContext(builder.Options);
        }
    }
}
