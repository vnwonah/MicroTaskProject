using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MT_NetCore_Common.Models;
using MTNetCoreData.Entities;

namespace MT_NetCore_Common.Interfaces
{
    public interface ICatalogRepository
    {
        Task<List<TenantModel>> GetAllTenants();
        Task<TenantModel> GetTenant(string tenantName);
        bool Add(Tenant tenant);
    }
}
