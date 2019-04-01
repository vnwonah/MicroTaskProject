using System;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using MT_NetCore_Data.Entities;

namespace MT_NetCore_Data.TenantsDB
{
    public partial class TenantDbContext : DbContext
    {
       
        public TenantDbContext(ShardMap shardMap, int shardingKey, string connectionStr) :
           base(CreateDdrConnection(shardMap, shardingKey, connectionStr))
        {

        }

        public TenantDbContext(DbContextOptions options) 
            : base(options)
        {
        }


        /// <summary>
        /// Creates the DDR (Data Dependent Routing) connection.
        /// </summary>
        /// <param name="shardMap">The shard map.</param>
        /// <param name="shardingKey">The sharding key.</param>
        /// <param name="connectionStr">The connection string.</param>
        /// <returns></returns>
        private static DbContextOptions CreateDdrConnection(ShardMap shardMap, int shardingKey, string connectionStr)
        {
            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);

            // Set TenantId in SESSION_CONTEXT to shardingKey to enable Row-Level Security filtering
            SqlCommand cmd = sqlConn.CreateCommand();
            cmd.CommandText = @"exec sp_set_session_context @key=N'Id', @value=@shardingKey";
            cmd.Parameters.AddWithValue("@shardingKey", shardingKey);
            cmd.ExecuteNonQuery();

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            var options = optionsBuilder.UseSqlServer(sqlConn).Options;

            return options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }

    }
}
