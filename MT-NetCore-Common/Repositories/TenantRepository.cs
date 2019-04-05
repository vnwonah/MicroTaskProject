using System;
using MT_NetCore_Common.Interfaces;

namespace MT_NetCore_Common.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private string v;

        public TenantRepository(string v)
        {
            this.v = v;
        }
    }
}
