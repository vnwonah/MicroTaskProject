using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_NetCore_API.Interfaces;
using Newtonsoft.Json.Linq;

namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RecordController : BaseController
    {
        public RecordController(
            IRequestContext requestContext) 
            : base(requestContext)
        {
                
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]JObject model)
        {
           return new OkResult();
        }
    }
}
