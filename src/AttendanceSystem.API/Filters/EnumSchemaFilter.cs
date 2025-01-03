using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumDescriptions = new List<OpenApiString>();
            foreach (var field in context.Type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var description = field.GetCustomAttribute<EnumMemberAttribute>()?.Value
                                  ?? field.Name;

                enumDescriptions.Add(new OpenApiString($"{field.Name} = {description}"));
            }

            schema.Enum = enumDescriptions.Cast<IOpenApiAny>().ToList();
        }
    }
}