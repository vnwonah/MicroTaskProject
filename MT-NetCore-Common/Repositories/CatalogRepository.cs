using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Common.Mapping;
using MT_NetCore_Common.Models;
using MT_NetCore_Data.CatalogDB;
using MT_NetCore_Data.Entities;

namespace MT_NetCore_Common.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly CatalogDbContext _catalogDbContext;

        public CatalogRepository(
            CatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }
        public async Task<List<TenantModel>> GetAllTenants()
        {
            var allTenantsList = await _catalogDbContext.Tenants.ToListAsync();

            if (allTenantsList.Count > 0)
            {
                return allTenantsList.Select(tenant => tenant.ToTenantModel()).ToList();
            }

            return null;
        }

        public async Task<TenantModel> GetTenant(string tenantName)
        {
            var tenants = await _catalogDbContext.Tenants.Where(i => Regex.Replace(i.Name.ToLower(), @"\s+", "") == tenantName).ToListAsync();

            if (tenants.Any())
            {
                var tenant = tenants.FirstOrDefault();
                return tenant?.ToTenantModel();
            }

            return null;
        }

        public bool Add(Tenant tenant)
        {
            _catalogDbContext.Tenants.Add(tenant);
            _catalogDbContext.SaveChangesAsync();

            return true;
        }
    }
}
