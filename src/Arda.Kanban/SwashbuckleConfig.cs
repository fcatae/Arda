using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Arda.Kanban
{
    public class MultipleOperationsWithSameVerbFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            ISchemaRegistry schemaRegistry = context.SchemaRegistry;
            ApiDescription apiDescription = context.ApiDescription;

            var overrideAttribute = apiDescription.ActionAttributes().FirstOrDefault(a => a is SwaggerOperationAttribute);

            // If the method is overwritten, return immediately
            if (overrideAttribute != null)
                return;

            // Configure the name according to the method name
            var controllerDescriptor = apiDescription.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            
            if(controllerDescriptor != null)
            {
                operation.OperationId = controllerDescriptor.ControllerName + "_" + controllerDescriptor.ActionName;
            }
        }
    }
}
