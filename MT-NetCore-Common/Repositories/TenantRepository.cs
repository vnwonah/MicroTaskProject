using System;
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
using MT_NetCore_Data.IdentityDB;
using MT_NetCore_Data.TenantsDB;
using MT_NetCore_Utils.Enums;
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

        public async Task<long> AddProjectToTeam(Project model, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                model.TeamId = tenantId;
                context.Projects.Add(model);
                await context.SaveChangesAsync();
                return model.Id;
            }
        }
        public async Task<long> AddProjectUser(long userId, long projectId, int tenantId, Role role)
        {
            using (var context = CreateContext(tenantId))
            {
                var pu = new ProjectUser { UserId = userId, ProjectId = projectId, UserRole = role};
                context.ProjectUsers.Add(pu);
                await context.SaveChangesAsync();
                return pu.ProjectId;
            }
        }


        public async Task<Project> GetProjectById(long id, int tenantId)
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
                 
                    .ToListAsync();

                return projects;
            }
        }
        #endregion

        #region Form
        public async Task<long> AddFormToProject(Form model, long projectId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                model.ProjectId = projectId;
                context.Forms.Add(model);
                await context.SaveChangesAsync();
                return model.Id;
            }
        }

        public async Task<Form> GetFormById(long formId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var form = await context.Forms.FirstOrDefaultAsync(f => f.Id == formId);
                return form;
            }

        }

        public async Task<long> AddUserToForm(long userId, long formId, int tenantId, Role role)
        {
            using (var context = CreateContext(tenantId))
            {
                var fu = new FormUser { UserId = userId, FormId = formId, UserRole = role };
                context.FormUsers.Add(fu);
                await context.SaveChangesAsync();
                return fu.FormId;
            }
        }
        public async Task<List<Form>> GetAllFormsForUser(string email, int tenantId)
        {
            var user = await GetUserByEmailAsync(email, tenantId);

            using (var context = CreateContext(tenantId))
            {
                var forms = await context.Forms.Where(fm => fm.FormUsers.Any(u => u.UserId == user.Id)).ToListAsync();
                return forms;
            }
        }

        public async Task<Form> GetFormForUserByFormId(long userId, long formId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                return await context.Forms.FirstOrDefaultAsync(fm => fm.FormUsers.Any(u => u.UserId == userId && u.FormId == formId));
                
            }
        }

        public async Task<List<User>> GetFormUsers(long formId, int tenantId)
        {
            using(var context = CreateContext(tenantId))
            {
                return await context.Users.Where(u => u.FormUsers.Any(f => f.FormId == formId)).ToListAsync();
            }
        }

        public async Task<List<Form>> GetProjectForms(long projectId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var forms = await context.Forms.Where(f => f.ProjectId == projectId).ToListAsync();

                return forms;
            }
        }

        public async Task<List<Form>> GetProjectFormsForUserAsync(string email, long projectId, int tenantId)
        {
            var user = await GetUserByEmailAsync(email, tenantId);
            using (var context = CreateContext(tenantId))
            {
                var forms = await context.Forms.Where(fm => fm.ProjectId == projectId && fm.FormUsers.Any(u => u.UserId == user.Id)).ToListAsync();
                return forms;
            }
        }

        public async Task UpdateFormAsync(Form form, int tenantId)
        {
            using(var context = CreateContext(tenantId))
            {
                context.Forms.Update(form);
                await context.SaveChangesAsync();
            }
        } 



        #endregion

        #region Records

        public async Task<long> AddRecord(Record record, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                context.Records.Add(record);
                await context.SaveChangesAsync();
                return record.Id;
            }
        }

        public async Task<long> UpdateRecordStatus(long recordId, RecordStatus status, string message, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                await context.Database.ExecuteSqlCommandAsync(
                    $"EXEC sp_UpdateRecordStatus {recordId}, {status}, {message}");
                return recordId;
            }
        }

        public async Task<List<Record>> GetRecordsForForm(long formId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                return await context.Records.Where(r => r.FormId == formId).ToListAsync();
            }
        }

        public async Task<Record> GetRecordById(long recordId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                return await context.Records.FirstOrDefaultAsync(r => r.Id == recordId);
            }
        }

        public async Task<List<Record>> GetRejectedAndInvalidatedRecordsForUser(long userId, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                var records = await context.Records.Where(r => r.UserId == userId &&
                                                        ((r.Status == RecordStatus.Rejected ||
                                                         r.Status == RecordStatus.Invalidated)) && r.SentToDevice == false)
                    .Include(r => r.Location)
                    .ToListAsync();

                foreach (var rec in records)
                {
                    rec.SentToDevice = true;
                }

                await context.SaveChangesAsync();
                return records;
            }
        }
        #endregion

        #region Users

        public async Task<long> AddUserToTeam(User user, int tenantId)
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

        public async Task<long> UpdateUser(User model, int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                context.Users.Update(model);
                var res = await context.SaveChangesAsync();
                return model.Id;
            }
        }
        #endregion

        public async Task<object> GetTeamUsers(int tenantId)
        {
            using (var context = CreateContext(tenantId))
            {
                return await context.Users.ToListAsync();
            }
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
