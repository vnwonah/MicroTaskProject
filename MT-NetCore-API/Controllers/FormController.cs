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

        [HttpGet]
        public async Task<IActionResult> Get(long formId)
        {
            if (formId == 0) return BadRequest();
            var form = await _tenantRepository.GetFormById(formId, TenantId);
            return Ok(form);
        }
        
        [HttpGet(nameof(GetFormUsers))]
        public async Task<IActionResult> GetFormUsers(long formId)
        {
            if (formId == 0) return BadRequest();
            var users = await _tenantRepository.GetFormUsers(formId, TenantId);
            return Ok(users);
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

        [HttpPatch]
        public async Task<IActionResult> UpdateForm(UpdateFormModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                //update form
                var form = await _tenantRepository.GetFormById(model.FormId, TenantId);
                if (form == null) return NotFound();
                form.FormJson = model.FormJson.ToString();
                await _tenantRepository.UpdateFormAsync(form, TenantId);
                return Ok(new { id = form.Id, form_title = form.Title });
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateFormModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {

                var form = new Form { Name = model.FormName, FormJson = model.FormJson.ToString(), Title = model.FormTitle };

                var formId = await _tenantRepository.AddFormToProject(form, model.ProjectId, TenantId);

                var user = await _userService.GetCurrentUserAsync(TenantId);

                await _tenantRepository.AddUserToForm(user.Id, formId, TenantId, Role.SuperAdministrator);

                return Ok(new { id = formId, form_title = model.FormTitle });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }
    }
}
