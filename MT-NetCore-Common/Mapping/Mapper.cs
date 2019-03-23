using System;
using System.Net;
using MT_NetCore_Common.Models;
using MT_NetCore_Data.Entities;

namespace MT_NetCore_Common.Mapping
{
    public static class Mapper
    {
        #region Entity To Model Mapping
        public static TenantModel ToTenantModel(this Tenant tenantEntity)
        {
            string tenantIdInString = BitConverter.ToString(tenantEntity.Id);
            tenantIdInString = tenantIdInString.Replace("-", "");

            return new TenantModel
            {
                TenantId = ConvertByteKeyIntoInt(tenantEntity.Id),
                TenantName = tenantEntity.TenantName.ToLower().Replace(" ", ""),
                TenantIdInString = tenantIdInString
            };
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Converts the byte key into int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static int ConvertByteKeyIntoInt(byte[] key)
        {
            // Make a copy of the normalized array
            byte[] denormalized = new byte[key.Length];

            key.CopyTo(denormalized, 0);

            // Flip the last bit and cast it to an integer
            denormalized[0] ^= 0x80;

            return IPAddress.HostToNetworkOrder(BitConverter.ToInt32(denormalized, 0));
        }

        #endregion
    }
}
