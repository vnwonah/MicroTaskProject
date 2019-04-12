using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MT_NetCore_Data.TenantsDB
{
    public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {

        public TenantDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TenantDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("TenantDbConnection"));

            return new TenantDbContext(builder.Options);

        }
    }
}
