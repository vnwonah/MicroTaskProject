using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_NetCore_API.Models.RequestModels;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProjectController : Controller
    {
        private readonly ITenantRepository _tenantRepository;

        public ProjectController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateProjectModel model)
        {
            if (ModelState.IsValid)
            {
                var project = new Project { Name = model.ProjectName };
                var projectId = await _tenantRepository.AddProjectToTeam(project, 44444); //you need to find team Id!!!!!!!!!!!!!!!!!!!!!!!!!!

            }

            return BadRequest(ModelState);
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
