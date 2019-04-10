﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using Microsoft.Extensions.Configuration;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.RequestModels;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Common.Models;
using MT_NetCore_Common.Utilities;
using MT_NetCore_Data.Entities;
using static MT_NetCore_Common.Utilities.AppConfig;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TeamController : BaseController
    {
        private readonly IUserService _userService;
        public readonly IUtilities _utilities;
        private readonly IConfiguration _configuration;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ITenantRepository _tenantRepository;

        public TeamController(
            IUserService userService,
            IUtilities utilities,
            IConfiguration configuration,
            ICatalogRepository catalogRepository,
            ITenantRepository tenantRepository)
        {
            _userService = userService;
            _utilities = utilities;
            _configuration = configuration;
            _catalogRepository = catalogRepository;
            _tenantRepository = tenantRepository;
        }
        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {

            var user = await _userService.GetUserAsync();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string tenantId)
        {
            return new OkObjectResult(new TeamModel());
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]CreateTeamModel model)
        {
            if (ModelState.IsValid)
            {
                //TenantServerConfig tenantServerConfig, DatabaseConfig databaseConfig, CatalogConfig catalogConfig
                var databaseConfig = new DatabaseConfig
                {
                    DatabasePassword = _configuration["DatabaseOptions:DatabasePassword"],
                    DatabaseUser = _configuration["DatabaseOptions:DatabaseUser"],
                    DatabaseServerPort = Int32.Parse(_configuration["DatabaseOptions:DatabaseServerPort"]),
                    SqlProtocol = SqlProtocol.Tcp,
                    ConnectionTimeOut = Int32.Parse(_configuration["DatabaseOptions:ConnectionTimeOut"]),
                };

                var catalogConfig = new CatalogConfig
                {
                    ServicePlan = _configuration["DatabaseOptions:ServicePlan"],
                    CatalogDatabase = _configuration["DatabaseOptions:CatalogDatabase"],
                    CatalogServer = _configuration["DatabaseOptions:CatalogServer"], // + ".database.windows.net"
                };

                var tenantServerConfig = new TenantServerConfig
                {
                    TenantServer = _configuration["DatabaseOptions:CatalogServer"],// + ".database.windows.net",
                    TenantDatabase = _configuration["DatabaseOptions:TenantDatabase"],

                };


                //get last Id
                var team = new Team
                {
                    Id = _utilities.GetTenantKey(model.TenantName),
                    Name = model.TenantName,
                    LogoLink = model.TenantLogoLink,

                };

                var shard = Sharding.CreateNewShard(tenantServerConfig.TenantDatabase, tenantServerConfig.TenantServer, databaseConfig.DatabaseServerPort, null);
                await _tenantRepository.AddTeam(team);
                var x = await Sharding.RegisterNewShard(team.Id, "", shard);
                //check if key is in catalog
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
