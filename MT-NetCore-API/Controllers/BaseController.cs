using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using MT_NetCore_API.Interfaces;

namespace MT_NetCore_API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        private readonly IRequestContext _requestContext;

        protected int TenantId { get; private set; }
 
        public BaseController(IRequestContext requestContext)
        {
            _requestContext = requestContext;
            TenantId = _requestContext.TenantId;
        }

    }
}
