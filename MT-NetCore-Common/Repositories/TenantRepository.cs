using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Common.Utilities;
using MT_NetCore_Data.Entities;
using MT_NetCore_Data.TenantsDB;
using Newtonsoft.Json;

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

        #region Project

        public async Task<int> AddProjectToTeam(Project model, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                model.TeamId = tenantId;
                context.Projects.Add(model);
                await context.SaveChangesAsync();
                return model.Id;
            }
        }
        public async Task<int> AddProjectUser(int userId, int projectId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var pu = new ProjectUser { UserId = userId, ProjectId = projectId };
                context.ProjectUsers.Add(pu);
                await context.SaveChangesAsync();
                return pu.ProjectId;
            }
        }


        public async Task<Project> GetProjectById(int id, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var project = await context.Projects.FirstOrDefaultAsync(i => i.Id == id && i.TeamId == tenantId);
                return project;
            }
        }

        public async Task<List<Project>> GetUserProjects(string email, int tenantId)
        {
            var user = await GetUserByEmailAsync(email, tenantId);
            using (var context = CreateContext(tenantId))
            {
                var projects = await context.Projects.Where(p => p.ProjectUsers.Any(u => u.UserId == user.Id))
                    //.Select(
                    //p => new
                    //{
                    //    p.Id,
                    //    p.Name
                    //})
                    .ToListAsync();

                return projects;
            }
        }
        #endregion

        #region Form
        public async Task<int> AddFormToProject(Form model, int projectId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                model.ProjectId = projectId;
                context.Forms.Add(model);
                await context.SaveChangesAsync();
                return model.Id;
            }
        }

        public async Task<List<Form>> GetProjectForms(int projectId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var forms = await context.Forms.Where(f => f.ProjectId == projectId).ToListAsync();

                return forms;
            }
        }

        public async Task<List<Form>> GetProjectFormsForUserAsync(string email, int projectId, int tenantId)
        {
            var user = await GetUserByEmailAsync(email, tenantId);
            using (var context = CreateContext(tenantId))
            {
                var forms = await context.Forms.Where(fm => fm.ProjectId == projectId && fm.Users.Contains(user)).ToListAsync();
                return forms;
            }
        }

        #endregion

        #region Users

        public async Task<int> AddUserToTeam(User user, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                user.TeamId = tenantId;
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var user = await context.Users.FirstOrDefaultAsync(i => i.Email == email && i.TeamId == tenantId);
                return user;

            }
        }

        public async Task<int> UpdateUser(User model, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                context.Users.Attach(model);
                var res = await context.SaveChangesAsync();
                return model.Id;
            }
        }
        #endregion


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
                    try
                    {
                        using (SqlConnection sqlConn = Sharding.ShardMap.OpenConnectionForKey(teamId, _connectionString))
                        {
                            databaseName = sqlConn.Database;
                            databaseServerName = sqlConn.DataSource.Split(':').Last().Split(',').First();
                        }
                        var team = await context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);

                        return team;
                    }
                    catch (JsonSerializationException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                   
                }
                return null;
            }
        }

       
    }
}
