using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using MT_NetCore_Common.Interfaces;
using static MT_NetCore_Common.Utilities.AppConfig;

namespace MT_NetCore_Common.Utilities
{

    public class Utilities : IUtilities
    {
       
        public async void RegisterTenantShard(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig)
        {
            //get all database in devtenantserver
            var tenants = GetAllTenantNames(tenantServerConfig, databaseConfig);

            var connectionString = new SqlConnectionStringBuilder
            {
                UserID = databaseConfig.DatabaseUser,
                Password = databaseConfig.DatabasePassword,
                ApplicationName = "EntityFramework",
                ConnectTimeout = databaseConfig.ConnectionTimeOut
            };

            Shard shard = Sharding.CreateNewShard(tenantServerConfig.TenantDatabase, tenantServerConfig.TenantServer, databaseConfig.DatabaseServerPort, catalogConfig.ServicePlan);

            foreach (var tenant in tenants)
            {
                var tenantId = GetTenantKey(tenant);
                _ = await Sharding.RegisterNewShard(tenantId, catalogConfig.ServicePlan, shard);
            }
        }

        public byte[] ConvertIntKeyToBytesArray(int key)
        {
            byte[] normalized = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(key));

            // Maps Int32.Min - Int32.Max to UInt32.Min - UInt32.Max.
            normalized[0] ^= 0x80;

            return normalized;
        }

      

        private List<string> GetAllTenantNames(TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig)
        {
            List<string> list = new List<string>();

            string conString = $"Server={databaseConfig.SqlProtocol}:{tenantServerConfig.TenantServer},{databaseConfig.DatabaseServerPort};Database={tenantServerConfig.TenantDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=False;Connection Timeout={databaseConfig.ConnectionTimeOut};";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Name from Teams", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString().ToLower().Replace(" ", ""));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Generates the tenant Id using MD5 Hashing.
        /// </summary>
        /// <param name="tenantName">Name of the tenant.</param>
        /// <returns></returns>

        public int GetTenantKey(string tenantName)
        {
            var normalizedTenantName = tenantName.Replace(" ", string.Empty).ToLower();

            //Produce utf8 encoding of tenant name 
            var tenantNameBytes = Encoding.UTF8.GetBytes(normalizedTenantName);

            //Produce the md5 hash which reduces the size
            MD5 md5 = MD5.Create();
            var tenantHashBytes = md5.ComputeHash(tenantNameBytes);

            //Convert to integer for use as the key in the catalog 
            int tenantKey = BitConverter.ToInt32(tenantHashBytes, 0);

            return tenantKey;
        }
       
    }
}
