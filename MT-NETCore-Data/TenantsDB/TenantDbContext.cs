using System;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public TenantDbContext(DbContextOptions options) : base(options)
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
            try
            {
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            // Ask shard map to broker a validated connection for the given key
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });
            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(pu => pu.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);           
                
            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(pu => pu.ProjectUsers)
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FormUser>()
                .HasKey(fu => new { fu.FormId, fu.UserId });
            modelBuilder.Entity<FormUser>()
                .HasOne(fu => fu.Form)
                .WithMany(fu => fu.FormUsers)
                .HasForeignKey(fu => fu.FormId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FormUser>()
                .HasOne(fu => fu.User)
                .WithMany(fu => fu.FormUsers)
                .HasForeignKey(fu => fu.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }
        public virtual DbSet<FormUser> FormUsers { get; set; }

    }
}
