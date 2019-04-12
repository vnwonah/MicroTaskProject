using System;
using Microsoft.AspNetCore.Http;
using MT_NetCore_API.Interfaces;

namespace MT_NetCore_API.Services
{
public sealed class RequestContextAdapter : IRequestContext
{
    private readonly IHttpContextAccessor _accessor;

    public RequestContextAdapter(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public int TenantId
    {
        get
        {
            var val = _accessor?.HttpContext?.Request?.Headers["X-Tenant-Id"];
            return val.ToString() == null || val.ToString() == "default" ? 0 : Int32.Parse(val);
        }
    }
}
}
