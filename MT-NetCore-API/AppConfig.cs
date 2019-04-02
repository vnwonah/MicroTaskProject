using System;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace MT_NetCore_API
{
    /// <summary>
    /// Common database settings
    /// </summary>
    public class DatabaseConfig
    {
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }
        public int DatabaseServerPort { get; set; }
        public int ConnectionTimeOut { get; set; }
        public SqlProtocol SqlProtocol { get; set; }
    }

    /// <summary>
    /// The catalog settings
    /// </summary>
    public class CatalogConfig
    {
        public string CatalogServer { get; set; }
        public string CatalogDatabase { get; set; }
        public string ServicePlan { get; set; }
    }

    /// <summary>
    /// The Tenant server configs
    /// </summary>
    public class TenantServerConfig
    {
        public string TenantServer { get; set; }
        public string TenantDatabase { get; set; }
    }

    /// <summary>
    /// The tenant configs
    /// </summary>
    public class TenantConfig
    {
        public int TenantId { get; set; }
        public string TenantIdInString { get; set; }
        public string User { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseServerName { get; set; }
    }
}
