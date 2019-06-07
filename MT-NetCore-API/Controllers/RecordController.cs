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
            if (!ModelState.IsValid) return BadRequest();
            throw new NotImplementedException();
        }


        [HttpPatch("reject")]
        public async Task<IActionResult> Reject(ChangeRecordStatusModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            throw new NotImplementedException();
        }

        [HttpPatch("invalidate")]
        public async Task<IActionResult> Invalidate(ChangeRecordStatusModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            throw new NotImplementedException();
        }

        [HttpPatch("delete")]
        public async Task<IActionResult> Delete(ChangeRecordStatusModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            throw new NotImplementedException();
        }
    }
}
