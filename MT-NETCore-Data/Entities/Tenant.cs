namespace MTNetCoreData.Entities
{
    public class Tenant : BaseEntity
    {
        public string TenantId { get; set; }

        public string TenantName { get; set; }
    }
}
