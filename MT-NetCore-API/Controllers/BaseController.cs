using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MT_NetCore_API.Extensions;

namespace MT_NetCore_API.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }

        /*
        protected void SetTenantConfig(int tenantId, string tenantIdInString)
        {
            var host = HttpContext.Request.Host.ToString();

            var tenantConfig = PopulateTenantConfigs(tenantId, tenantIdInString, host);

            if (tenantConfig != null)
            {
                var tenantConfigs = HttpContext.Session.GetObjectFromJson<List<TenantConfig>>("TenantConfigs");
                if (tenantConfigs == null)
                {
                    tenantConfigs = new List<TenantConfig>
                    {
                        tenantConfig
                    };
                    HttpContext.Session.SetObjectAsJson("TenantConfigs", tenantConfigs);
                }
                else
                {
                    var tenantsInfo = tenantConfigs.Where(i => i.TenantId == tenantId);

                    if (!tenantsInfo.Any())
                    {
                        tenantConfigs.Add(tenantConfig);
                        HttpContext.Session.SetObjectAsJson("TenantConfigs", tenantConfigs);
                    }
                    else
                    {
                        for (var i = 0; i < tenantConfigs.Count; i++)
                        {
                            if (tenantConfigs[i].TenantId == tenantId)
                            {
                                tenantConfigs[i] = tenantConfig;
                                HttpContext.Session.SetObjectAsJson("TenantConfigs", tenantConfigs);
                                break;
                            }
                        }
                    }
                }

            }
        }

        private TenantConfig PopulateTenantConfigs(int tenantId, string tenantIdInString, string host)
        {
            try
            {
                //get user from url
                string user;
                if (host.Contains("localhost"))
                {
                    user = "testuser";
                }
                else
                {
                    string[] hostpieces = host.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    user = hostpieces[2];
                }

                //get the venue details and populate in config settings
                var venueDetails = (_tenantRepository.GetVenueDetails(tenantId)).Result;
                var venueTypeDetails =
                    (_tenantRepository.GetVenueType(venueDetails.VenueType, tenantId)).Result;
                var countries = (_tenantRepository.GetAllCountries(tenantId)).Result;

                //get country language from db 
                var country = (_tenantRepository.GetCountry(venueDetails.CountryCode, tenantId)).Result;
                RegionInfo regionalInfo = new RegionInfo(country.Language);

                return new TenantConfig
                {
                    DatabaseName = venueDetails.DatabaseName,
                    DatabaseServerName = venueDetails.DatabaseServerName,
                    VenueName = venueDetails.VenueName,
                    BlobImagePath = blobPath + venueTypeDetails.VenueType + "-user.jpg",
                    EventTypeNamePlural = venueTypeDetails.EventTypeShortNamePlural.ToUpper(),
                    TenantId = tenantId,
                    TenantName = venueDetails.VenueName.ToLower().Replace(" ", ""),
                    Currency = regionalInfo.CurrencySymbol,
                    TenantCulture =
                        (!string.IsNullOrEmpty(venueTypeDetails.Language)
                            ? venueTypeDetails.Language
                            : defaultCulture),
                    TenantCountries = countries,
                    TenantIdInString = tenantIdInString,
                    User = user
                };
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, "Error in populating tenant config.");
            }
            return null;
        }*/
    }
}
