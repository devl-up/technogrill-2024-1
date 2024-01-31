using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TechnoGrill.Infrastructure.Web.SchemaFilters;

public sealed class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }

        foreach (var property in schema.Properties)
        {
            if (property.Value.Nullable)
            {
                continue;
            }

            schema.Required.Add(property.Key);
        }
    }
}