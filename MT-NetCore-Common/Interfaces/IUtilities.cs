using static MT_NetCore_Common.Utilities.AppConfig;

namespace MT_NetCore_Common.Interfaces
{
    public interface IUtilities
    {
        void RegisterTenantShard(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig);

        byte[] ConvertIntKeyToBytesArray(int key);

        int GetTenantKey(string tenantName);
    }
}
