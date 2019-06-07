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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RecordController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITenantRepository _tenantRepository;
        public RecordController(
            IRequestContext requestContext,
            IUserService userService,
            ITenantRepository tenantRepository) 
            : base(requestContext)
        {
            _userService = userService;
            _tenantRepository = tenantRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRecordModel model)
        {
            try
            {
                //check that user is in team
                var user = await _userService.GetCurrentUserAsync(TenantId);
                if (user == null) return Unauthorized();

                //check that user has access to form
               var form = await _tenantRepository.GetFormForUserByFormId(user.Id, model.FormId, TenantId);
               if (form == null) return Unauthorized();

                var record = new Record
                {
                    FormId = model.FormId,
                    RecordJson = JsonConvert.SerializeObject(model.ResponseJson),
                    Status = RecordStatus.Submitted,
                    Location = new Location
                    {
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                    },
                    UserId = user.Id,
                    Message = "Submitted"
                };
                await _tenantRepository.AddRecord(record, TenantId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            
        }

        [HttpPatch("approve")]
        public async Task<IActionResult> Approve(ChangeRecordStatusModel model)
        {
            //confirm if user can perform action Vincent!

            if (!ModelState.IsValid) return BadRequest();
            await _tenantRepository.UpdateRecordStatus(model.RecordId, RecordStatus.Approved, model.Message, TenantId);
            return new OkObjectResult(new {record_id = model.RecordId});
        }


        [HttpPatch("reject")]
        public async Task<IActionResult> Reject(ChangeRecordStatusModel model)
        {
            //confirm if user can perform action Vincent!

            if (!ModelState.IsValid) return BadRequest();
            await _tenantRepository.UpdateRecordStatus(model.RecordId, RecordStatus.Rejected, model.Message, TenantId);
            return new OkObjectResult(new { record_id = model.RecordId });
        }

        [HttpPatch("invalidate")]
        public async Task<IActionResult> Invalidate(ChangeRecordStatusModel model)
        {
            //confirm if user can perform action Vincent!

            if (!ModelState.IsValid) return BadRequest();
            await _tenantRepository.UpdateRecordStatus(model.RecordId, RecordStatus.Invalidated, model.Message, TenantId);
            return new OkObjectResult(new { record_id = model.RecordId });
        }

        [HttpPatch("delete")]
        public async Task<IActionResult> Delete(ChangeRecordStatusModel model)
        {
            //confirm if user can perform action Vincent!

            if (!ModelState.IsValid) return BadRequest();
            await _tenantRepository.UpdateRecordStatus(model.RecordId, RecordStatus.Deleted, model.Message, TenantId);
            return new OkObjectResult(new { record_id = model.RecordId });
        }
    }
}
