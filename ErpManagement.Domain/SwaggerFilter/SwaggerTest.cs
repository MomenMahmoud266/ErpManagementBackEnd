namespace ErpManagement.Domain.SwaggerFilter;

public class SwaggerTest : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties is null)
            return;

        var ignoreDataMemberProperties = context.Type.GetProperties()
            .Where(t => t.GetCustomAttribute<IgnoreDataMemberAttribute>() is not null);
        foreach (var ignoreDataMemberProperty in ignoreDataMemberProperties)
        {
            var propertyToHide = schema.Properties.Keys
                .SingleOrDefault(x => x.Equals(ignoreDataMemberProperty.Name, StringComparison.CurrentCultureIgnoreCase));

            if (propertyToHide is not null)
                schema.Properties.Remove(propertyToHide);
        }
    }
}
