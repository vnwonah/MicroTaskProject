using MT_NetCore_Common.Utilities;

namespace MT_NetCore_Common.Interfaces
{
    public interface IUtilities
    {
        void RegisterTenantShard(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig, bool resetEventDate);

        byte[] ConvertIntKeyToBytesArray(int key);

    }
}
