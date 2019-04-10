using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Common.Utilities;
using MT_NetCore_Data.Entities;
using MT_NetCore_Data.TenantsDB;

namespace MT_NetCore_Common.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public TenantRepository(
            string connectionString,
            IConfiguration configuration)
        {
            _connectionString = connectionString;
            _configuration = configuration;
        }

        private TenantDbContext CreateContext(int tenantId)
        {
            return new TenantDbContext(Sharding.ShardMap, tenantId, _connectionString);
        }


        public async Task<Team> AddTeam(Team team)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(_configuration.GetConnectionString("TenantDbConnection"));

            using (var context = new TenantDbContext(builder.Options))
            {
                //remove Identity Insert
                await context.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Teams] ON");

                //Create Team in DB
                int res = await context.Database.ExecuteSqlCommandAsync($"EXEC sp_NewTeam {team.Id}, {team.Name}, {team.LogoLink}, '20120618 10:34:09 AM'");

                if (res == 1) //MAGIC NUMBER! Insertion Successful!
                    return team;

            }
            return null;
        }

        public async Task<Team> GetTeamDetailsAsync(int teamId)
        {
            using (var context = CreateContext(teamId))
            {
                //get database name
                string databaseName, databaseServerName;
                PointMapping<int> mapping;

                if (Sharding.ShardMap.TryGetMappingForKey(teamId, out mapping))
                {
                    using (SqlConnection sqlConn = Sharding.ShardMap.OpenConnectionForKey(teamId, _connectionString))
                    {
                        databaseName = sqlConn.Database;
                        databaseServerName = sqlConn.DataSource.Split(':').Last().Split(',').First();
                    }
                    var team = await context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);

                    /*
                    if (venue != null)
                    {
                        var venueModel = venue.ToVenueModel();
                        venueModel.DatabaseName = databaseName;
                        venueModel.DatabaseServerName = databaseServerName;
                        return venueModel;
                    }
                    */
                    return team;
                }
                return null;
            }
        }

       
    }
}
