using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.RequestModels;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Data.Entities;
using MT_NetCore_Utils.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProjectController : BaseController
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserService _userService;

        public ProjectController(
            ITenantRepository tenantRepository,
            IUserService userService,
            IRequestContext requestContext) : base(requestContext)
        {
            _tenantRepository = tenantRepository;
            _userService = userService;
        }


        // GET api/values/5
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var project = await _tenantRepository.GetProjectById(id, TenantId);
            if (project != null)
                return Ok(project);
            return NotFound();
        }


        /// <summary>
        /// Adds a new Project to a Team
        /// </summary>
        /// <remarks>The header value 'X-Tenant-Id' must be passed in the http request header</remarks>
        /// <response code="200">Project added successfully</response>
        /// <response code="400">Something went wrong</response>
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateProjectModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var project = new Project { Name = model.ProjectName };

                    var projectId = await _tenantRepository.AddProjectToTeam(project, TenantId); //you need to find team Id!!!!!!!!!!!!!!!!!!!!!!!!!!

                    var user = await _userService.GetCurrentUserAsync(TenantId);

                    await _tenantRepository.AddProjectUser(user.Id, projectId, TenantId, Role.SuperAdministrator);

                    return Ok(new { id = projectId, project_name = model.ProjectName, users = new List<User> { user } });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message});
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
