using Microsoft.EntityFrameworkCore;
using MTNETStdData.Entities;

namespace MTNETStdData.CatalogDB
{
    public partial class CatalogDbContext : DbContext
    {
        public virtual DbSet<Tenant> Tenants { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) :
         base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

    }
}
