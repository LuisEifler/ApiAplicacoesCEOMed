using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.ParamModelsApiAplicacoes.OneHealth;
using APICeomedAplicacoes.Uteis.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace APICeomedAplicacoes.Uteis.Swagger
{
    public class ExampleValueFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {

            if (schema?.Properties == null || context.Type == null)
                return;

            foreach (var prop in context.Type.GetProperties())
            {
                var attr = (ExampleValue?)Attribute.GetCustomAttribute(prop, typeof(ExampleValue));

                if (schema.Properties.TryGetValue(prop.Name.FirstLetterLower(), out var propSchema))
                {
                    var propType = prop.PropertyType;

                    if (attr != null)
                    {
                        if (propType == typeof(string))
                            propSchema.Example = new OpenApiString(attr.Value.ToString());
                        else if (propType == typeof(int) || propType == typeof(int?))
                            propSchema.Example = new OpenApiInteger((int)attr.Value);
                        else if (propType == typeof(long) || propType == typeof(long?))
                            propSchema.Example = new OpenApiLong((long)attr.Value);
                        else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                            propSchema.Example = new OpenApiString(((DateTime)attr.Value).ToString("yyyy-MM-dd"));
                        else if (propType == typeof(TimeSpan) || propType == typeof(TimeSpan?))
                            propSchema.Example = new OpenApiString(((TimeSpan)attr.Value).ToString());
                    }
                    else
                    {
                        // valores padrão (fallback)
                        if (propType == typeof(string))
                            propSchema.Example = new OpenApiString("String");
                        else if (propType == typeof(int) || propType == typeof(int?))
                            propSchema.Example = new OpenApiInteger(1);
                        else if (propType == typeof(Int16) || propType == typeof(Int16?))
                            propSchema.Example = new OpenApiInteger(1);
                        else if (propType == typeof(long) || propType == typeof(long?))
                            propSchema.Example = new OpenApiLong(1234);
                        else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
                            propSchema.Example = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd"));
                        else if (propType == typeof(TimeSpan) || propType == typeof(TimeSpan?))
                            propSchema.Example = new OpenApiString(DateTime.Now.ToString("HH:mm:ss"));
                    }
                }
            }


        }

    }

    public class FromQueryExampleOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var paramDesc in context.ApiDescription.ParameterDescriptions)
            {
                var paramType = paramDesc.Type;
                if(paramDesc.Source.DisplayName == "Query")
                {
                    var prop = paramDesc.ModelMetadata.ContainerType.GetProperty(paramDesc.Name);
                    ExampleValue exampleAttr = (ExampleValue)Attribute.GetCustomAttribute(prop,typeof(ExampleValue));
                    if (exampleAttr != null)
                    {
                        var swaggerParam = operation.Parameters.FirstOrDefault(p =>
                            string.Equals(p.Name, paramDesc.Name, StringComparison.OrdinalIgnoreCase));
                        if (swaggerParam != null)
                        {
                            if (paramType == typeof(string))
                                swaggerParam.Example = new OpenApiString(exampleAttr.Value.ToString());
                            else if (paramType == typeof(int) || paramType == typeof(int?))
                                swaggerParam.Example = new OpenApiInteger((int)exampleAttr.Value);
                            else if (paramType == typeof(long) || paramType == typeof(long))
                                swaggerParam.Example = new OpenApiLong((long)exampleAttr.Value);
                            else if (paramType == typeof(DateTime) || paramType == typeof(DateTime?))
                                swaggerParam.Example = new OpenApiString(exampleAttr.Value.ToString());
                            else if (paramType == typeof(TimeSpan) || paramType == typeof(TimeSpan?))
                                swaggerParam.Example = new OpenApiString(exampleAttr.Value.ToString());
                        }
                        else
                        {
                            if (paramType == typeof(string))
                                swaggerParam.Example = new OpenApiString("String");
                            else if (paramType == typeof(int) || paramType == typeof(int))
                                swaggerParam.Example = new OpenApiInteger(1);
                            else if (paramType == typeof(Int16) || paramType == typeof(Int16?))
                                swaggerParam.Example = new OpenApiInteger(1);
                            else if (paramType == typeof(Int64) || paramType == typeof(Int64?))
                                swaggerParam.Example = new OpenApiLong(1234);
                            else if (paramType == typeof(DateTime) || paramType == typeof(DateTime?))
                                swaggerParam.Example = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd"));
                            else if (paramType == typeof(TimeSpan) || paramType == typeof(TimeSpan?))
                                swaggerParam.Example = new OpenApiString(DateTime.Now.ToString("HH:mm:ss"));
                        }
                    }
                }
               
            }
        }
    }


}
