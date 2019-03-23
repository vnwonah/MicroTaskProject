using System;
namespace MT_NetCore_Common.Interfaces
{
    public interface IUtilities
    {
        void RegisterTenantShard(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig, bool resetEventDate);

        byte[] ConvertIntKeyToBytesArray(int key);

    }
}
