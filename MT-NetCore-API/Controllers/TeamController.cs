using System;
using System.Collections.Generic;
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
            ITenantRepository tenantRepository,
            IRequestContext requestContext) : base(requestContext)
        {
            _userService = userService;
            _utilities = utilities;
            _configuration = configuration;
            _catalogRepository = catalogRepository;
            _tenantRepository = tenantRepository;
        }
      
        /*
        [HttpGet("{id}")]
        public IActionResult Get(string tenantId)
        {
            return new OkObjectResult(new TeamModel());
        }
        */

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateTeamModel model)
        {
            //TODO: Implement Detailed Error Checking
            if (ModelState.IsValid)
            {
                try
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

                    var team = new Team
                    {
                        Id = _utilities.GetTenantKey(model.TenantName),
                        Name = model.TenantName,
                        LogoLink = model.TenantLogoLink,
                        
                    };



                    //Create Shard, Add Team and Register Tenant against shard
                    var shard = Sharding.CreateNewShard(tenantServerConfig.TenantDatabase, tenantServerConfig.TenantServer, databaseConfig.DatabaseServerPort, null);
                    await _tenantRepository.AddTeam(team);
                    var x = await Sharding.RegisterNewShard(team.Id, "", shard);

                    //Add first user to team. Team Owner!
                    var applicationUser = await _userService.GetApplicationUserAsync();
                    var user = new User { ApplicationUserId = applicationUser.Id, Email = applicationUser.Email };
                    await _tenantRepository.AddUserToTeam(user, team.Id);


                    return Ok(new { team_id = team.Id, team_name = team.Name });
                }
                catch (Exception ex)
                {
                    //TODO: Log Error
                    return BadRequest(new { Error = ex.Message });
                }
               
            }

            return BadRequest(ModelState);
        }
        /*

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
        */
    }
}
