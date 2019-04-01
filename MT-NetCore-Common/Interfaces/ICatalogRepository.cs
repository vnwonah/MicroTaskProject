using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MT_NetCore_Common.Models;
using MT_NetCore_Data.Entities;

namespace MT_NetCore_Common.Interfaces
{
    public interface ICatalogRepository
    {
        Task<List<TenantModel>> GetAllTenants();
        Task<TenantModel> GetTenant(string tenantName);
        bool Add(Team tenant);
    }
}
