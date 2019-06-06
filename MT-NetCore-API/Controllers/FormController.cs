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
using MT_NetCore_Utils.Enums;

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
        private ICompressionSystem _compressionSystem;
        

        public FormController(
            ITenantRepository tenantRepository,
            IUserService userService,
            IRequestContext requestContext,
            ICompressionSystem compressionSystem) 
            : base(requestContext)
        {
            _tenantRepository = tenantRepository;
            _userService = userService;
            _compressionSystem = compressionSystem;
        }

        [HttpGet("GetAllFormsForUser")]
        public async Task<IActionResult> GetAllForms()
        {
            var user = await _userService.GetCurrentUserAsync(TenantId);
            var forms = await _tenantRepository.GetAllFormsForUser(user.Email, TenantId);
            if (forms.Any())
                return Ok(forms);
            return NoContent();
        }

        [HttpGet("GetProjectForms")]
        public async Task<IActionResult> GetProjectForms(int projectId)
        {
            var forms = await _tenantRepository.GetProjectForms(projectId, TenantId);
            if (forms.Any())
                return Ok(forms);
            return NoContent();
        }

        [HttpGet("GetProjectFormsForUser")]
        public async Task<IActionResult> GetProjectFormsForUser(string email, int projectId)
        {
            var forms = await _tenantRepository.GetProjectForms(projectId, TenantId);
            if (forms.Any())
                return Ok(forms);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var form = new Form { Name = model.FormName, FormJson = model.FormJson.ToString()};

                    var formId = await _tenantRepository.AddFormToProject(form, model.ProjectId, TenantId);

                    var user = await _userService.GetCurrentUserAsync(TenantId);

                    await _tenantRepository.AddUserToForm(user.Id, formId, TenantId, Role.SuperAdministrator);

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
