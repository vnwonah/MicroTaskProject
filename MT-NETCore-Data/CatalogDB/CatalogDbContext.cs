using Microsoft.EntityFrameworkCore;
using MT_NetCore_Data.Entities;

namespace MT_NetCore_Data.CatalogDB
{
    public partial class CatalogDbContext : DbContext
    {
        public virtual DbSet<Team> Tenants { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) :
         base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

    }
}
