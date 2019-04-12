using System;
using System.Threading.Tasks;
using MT_NetCore_Data.Entities;

namespace MT_NetCore_Common.Interfaces
{
    public interface ITenantRepository
    {
        #region Teams

        Task<Team> AddTeam(Team team);
        Task<Team> GetTeamDetailsAsync(int teamId);

        #endregion

        #region Users

        Task<int> AddUserToTeam(User user, int tenantId);
        Task<User> GetUserByEmailAsync(string email, int tenantId);
        Task<bool> UpdateUser(User model, int tenantId);

        #endregion

        #region Projects

        Task<int> AddProjectToTeam(Project model, int tenantId);

        #endregion

        #region Events

        //Task<List<EventModel>> GetEventsForTenant(int tenantId);
        //Task<EventModel> GetEvent(int eventId, int tenantId);

        #endregion

        #region Sections

        //Task<List<SectionModel>> GetSections(List<int> sectionIds, int tenantId);
        //Task<SectionModel> GetSection(int sectionId, int tenantId);

        #endregion

        #region TicketPurchases

        //Task<int> AddTicketPurchase(TicketPurchaseModel ticketPurchaseModel, int tenantId);
        #endregion

        #region Tickets

        //Task<bool> AddTickets(List<TicketModel> ticketModel, int tenantId);
        //Task<int> GetTicketsSold(int sectionId, int eventId, int tenantId);

        #endregion

        #region Venues

        //Task<VenuesModel> GetVenueDetails(int tenantId);

        #endregion

        #region VenueTypes

        //Task<VenueTypeModel> GetVenueType(string venueType, int tenantId);

        #endregion

    }
}
