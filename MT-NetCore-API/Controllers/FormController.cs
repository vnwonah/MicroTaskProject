using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.RequestModels;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FormController : BaseController
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserService _userService;

        public FormController(
            ITenantRepository tenantRepository,
            IUserService userService,
            IRequestContext requestContext) : base(requestContext)
        {
            _tenantRepository = tenantRepository;
            _userService = userService;
        }

        [HttpGet("GetProjectForms")]
        public async Task<IActionResult> GetProjectForms(int projectId)
        {
            var forms = await _tenantRepository.GetProjectForms(projectId, TenantId);
            if (forms != null)
                return Ok(forms);
            return NotFound();
        }

        [HttpGet("GetProjectFormsForUser")]
        public async Task<IActionResult> GetProjectFormsForUser(string email, int projectId)
        {
            var forms = await _tenantRepository.GetProjectForms(projectId, TenantId);
            if (forms != null)
                return Ok(forms);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var form = new Form { Name = model.FormName };

                    var formId = await _tenantRepository.AddFormToProject(form, model.ProjectId, TenantId);

                    return Ok(new { id = formId, form_name = model.FormName });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }

            }

            return BadRequest(ModelState);
        }
    }
}
