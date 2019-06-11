using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MT_NetCore_Data.Entities;
using MT_NetCore_Utils.Enums;

namespace MT_NetCore_Common.Interfaces
{
    public interface ITenantRepository
    {
        #region Teams

        Task<Team> AddTeam(Team team);
        Task<Team> GetTeamDetailsAsync(int teamId);

        #endregion

        #region Users

        Task<long> AddUserToTeam(User user, int tenantId);
        Task<User> GetUserByEmailAsync(string email, int tenantId);
        Task<long> UpdateUser(User model, int tenantId);
        Task<List<Project>> GetUserProjects(string email, int tenantId);

        #endregion

        #region Projects

        Task<long> AddProjectToTeam(Project model, int tenantId);
        Task<long> AddProjectUser(long userId, long projectId, int tenantId, Role role);
        Task<Project> GetProjectById(long id, int tenantId);

        #endregion

        #region Forms

        Task<long> AddFormToProject(Form model, long projectId, int tenantId);
        Task<long> AddUserToForm(long userId, long formId, int tenantId, Role role);
        Task<List<Form>> GetProjectForms(long projectId, int tenantId);
        Task<List<Form>> GetProjectFormsForUserAsync(string email, long projectId, int tenantId);
        Task<List<Form>> GetAllFormsForUser(string email, int tenantId);
        Task<Form> GetFormForUserByFormId(long userId, long formId, int tenantId);
        Task<Form> GetFormById(long formId, int tenantId);
        Task UpdateFormAsync(Form form, int tenantId);
        #endregion

        #region Records

        Task<long> AddRecord(Record record, int tenantId);
        Task<long> UpdateRecordStatus(long recordId, RecordStatus status, string message, int tenantId);
        Task<List<Record>> GetRejectedAndInvalidatedRecordsForUser(long userId, int tenantId);

        #endregion

    }
}
