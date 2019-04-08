using System;
namespace MT_NetCore_API.Models.RequestModels
{
    public class CreateTeamModel
    {
        public string TenantName { get; set; }

        public string TenantLogoLink { get; set; }

        public string TenantCountry { get; set; }
    }
}
