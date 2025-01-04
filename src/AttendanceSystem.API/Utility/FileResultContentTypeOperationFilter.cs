using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AttendanceSystem.API.Utility
{
    public class FileResultContentTypeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the operation returns a FileResult
            if (context.MethodInfo.ReturnType == typeof(FileResult))
            {
                // Add a parameter to the Swagger UI for selecting the content type
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Content-Type",
                    In = ParameterLocation.Header,
                    Description = "The content type of the exported file.",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = Enum.GetValues(typeof(FileContentType)).Cast<FileContentType>().Select(t => new OpenApiString(t.ToString())).ToList<IOpenApiAny>()
                    }
                });
            }
        }
    }

    public enum FileContentType
    {
        CSV,
        PDF,
        XLSX
    }
}
