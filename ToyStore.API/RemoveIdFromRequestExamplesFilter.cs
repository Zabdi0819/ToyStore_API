using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RemoveIdFromRequestExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody == null) return;

        foreach (var kv in operation.RequestBody.Content.ToList())
        {
            var mediaType = kv.Value;
            var originalSchema = mediaType.Schema;
            if (originalSchema == null) continue;

            var resolved = ResolveReferencedSchema(originalSchema, context);

            var requestSchema = CloneSchemaExcludingReadOnly(resolved);

            mediaType.Schema = requestSchema;

            mediaType.Schema.Required?.Remove("id");

            mediaType.Example = CreateExampleFromSchema(mediaType.Schema);
            mediaType.Examples?.Clear();
        }
    }

    private OpenApiSchema ResolveReferencedSchema(OpenApiSchema schema, OperationFilterContext context)
    {
        if (schema.Reference == null) return schema;
        var id = schema.Reference.Id;
        if (context.SchemaRepository?.Schemas != null && context.SchemaRepository.Schemas.TryGetValue(id, out var referenced))
        {
            return referenced;
        }
        return schema;
    }

    private OpenApiSchema CloneSchemaExcludingReadOnly(OpenApiSchema src)
    {
        if (src == null) return null;
        var clone = new OpenApiSchema
        {
            Type = src.Type,
            Format = src.Format,
            Nullable = src.Nullable,
            Description = src.Description,
            Properties = new Dictionary<string, OpenApiSchema>()
        };

        if (src.Properties != null)
        {
            foreach (var kv in src.Properties)
            {
                if (kv.Value != null && kv.Value.ReadOnly == true) continue;
                clone.Properties[kv.Key] = kv.Value;
            }
        }

        if (src.Required != null)
        {
            clone.Required = new SortedSet<string>(src.Required.Where(r =>
                !(src.Properties != null && src.Properties.TryGetValue(r, out var p) && p.ReadOnly == true)
            ));
        }

        return clone;
    }

    private IOpenApiAny CreateExampleFromSchema(OpenApiSchema schema)
    {
        var obj = new OpenApiObject();
        if (schema?.Properties == null) return obj;

        foreach (var prop in schema.Properties)
        {
            var s = prop.Value;
            var name = prop.Key;
            switch (s.Type)
            {
                case "string":
                    obj.Add(name, new OpenApiString(name + "_example"));
                    break;
                case "integer":
                    obj.Add(name, new OpenApiInteger(1));
                    break;
                case "number":
                    obj.Add(name, new OpenApiDouble(1));
                    break;
                case "boolean":
                    obj.Add(name, new OpenApiBoolean(true));
                    break;
                default:
                    obj.Add(name, new OpenApiString(name + "_example"));
                    break;
            }
        }

        return obj;
    }
}
