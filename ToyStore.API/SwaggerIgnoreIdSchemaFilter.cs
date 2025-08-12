using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ToyStore.API.Models;

public class SwaggerIgnoreIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Product))
        {
            if (schema.Properties == null) schema.Properties = new Dictionary<string, OpenApiSchema>();

            if (schema.Properties.ContainsKey("id"))
            {
                schema.Properties["id"].ReadOnly = true;
            }
            else
            {
                schema.Properties.Add("id", new OpenApiSchema
                {
                    Type = "integer",
                    Format = "int32",
                    ReadOnly = true
                });
            }
        }
    }
}
