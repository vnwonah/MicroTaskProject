using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MT_NetCore_API.Swagger
{
    public class HeaderFilter : IOperationFilter
    {
      
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "X-Tenant-Id",
                In = "header",
                Type = "integer",
                Required = false,
                Default = "default",
                Description = "Tenant Id for current tenant"
            });
        }
    }
}
