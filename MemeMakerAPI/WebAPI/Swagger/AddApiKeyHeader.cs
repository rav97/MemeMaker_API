using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Swagger
{
    public class AddApiKeyHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Api key for authorisation",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    #warning This walue might be outdated in future
                    Default = new OpenApiString("MjAyMi0xMS0xNSAxMTozMjo0MQ==")
                }
            });
        }
    }
}
