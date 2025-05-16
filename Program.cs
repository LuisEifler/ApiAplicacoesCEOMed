using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using APICeomedAplicacoes.Base;
using APICeomedAplicacoes.Conexao;
using APICeomedAplicacoes.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APICeomedAplicacoes.Uteis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;


var builder = WebApplication.CreateBuilder(args);

var secretKey = "06NFE1u8WoZrg4M9LITf7pSVigOcN3J5CrjOeMqLX0m1nZBsccegbtTk0NVhnqNOhNlL7VkKv4Gm8LCQ7fNz2FGHMTo8WRWZXKF1rz2lqiro=";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "ceomedaplicacoes",
            ValidAudience = "ceomedaplicacoes",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                Response response = Response.Error();
                response.AddError("ApiToken ausente ou inválido.");
                response.Code = 401;
                response.traceId = LogAPIAplicacoes.GravarLog(response, context.Request, context.Request.Method is "POST" or "PATCH" ? context.HttpContext.Items["RawBody"] as string : null).Result;
                var result = System.Text.Json.JsonSerializer.Serialize(response);

                return context.Response.WriteAsync(result);

            },
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                Response response = Response.Error();
                response.AddError("ApiToken ausente ou inválido.");
                response.Code = 401;
                
                response.traceId = LogAPIAplicacoes.GravarLog(response, context.Request, context.Request.Method is "POST" or "PATCH" ? context.HttpContext.Items["RawBody"] as string : null).Result;
                
                var result = System.Text.Json.JsonSerializer.Serialize(response);

                return context.Response.WriteAsync(result);
            }
        };
           
    });

TypeDescriptor.AddAttributes(typeof(int?), new TypeConverterAttribute(typeof(NullableIntConverter)));


builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var erros = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Campo = e.Key,
                    Erro = e.Value.Errors.First().ErrorMessage
                })
                ;

            Response response = new Response()
            {
                IsSuccess = false,
                message = "Parametrôs invalidos.",
                Code = 400
            };

            foreach (var erro in erros)
            {
                response.AddError($"O valor enviado para '{erro?.Campo}' é inválido.", "Parametrôs invalidos.");
            }
           
            response.traceId = LogAPIAplicacoes.GravarLog(response, context.HttpContext.Request, context.HttpContext.Request.Method is "POST" or "PATCH" ? context.HttpContext.Items["RawBody"] as string : null).Result;


            return new BadRequestObjectResult(response);
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Aplicações CEOMed", Version = "v2" });
    c.EnableAnnotations();
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token no formato: Bearer {seu_token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    //c.AddServer(new OpenApiServer
    //{
    //    Url = "https://ceomed.app.br:2025/api/v2",
    //    Description = "Externo CEOMed"
    //});

    //c.AddServer(new OpenApiServer
    //{
    //    Url = "https://192.168.10.252:2025/api/v2",
    //    Description = "interno CEOMed"
    //});
    //c.AddServer(new OpenApiServer
    //{
    //    Url = "https://localhost:7097//api/v2",
    //    Description = "Debug CEOMed"
    //});
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.WebHost.UseIISIntegration();

var app = builder.Build();



app.UseCors("AllowAllOrigins");

app.UseMiddleware<BodyLoggingMiddleware>();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Aplicações CEOMed");
    c.RoutePrefix = "api";
    c.DocumentTitle = "API Aplicações CEOMed";

    c.HeadContent = @"
        <link rel='icon' type='image/x-icon' href='/swagger-ui/ceoicon.ico' />
        <meta http-equiv='cache-control' content='no-cache' />
        <meta http-equiv='expires' content='0' />
        <meta http-equiv='pragma' content='no-cache' />
    ";    
    // CSS e JS customizados
    c.InjectStylesheet("../swagger-ui/custom.css");
    //c.InjectJavascript("../swagger-ui/custom.js");ss

    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List); // fecha os endpoints
    c.DefaultModelsExpandDepth(-1); // oculta os modelos
});


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Util.Log(" a88888b.                                             dP\r\nd8'   `88                                             88 \r\n88        .d8888b. .d8888b. 88d8b.d8b. .d8888b. .d888b88 \r\n88        88ooood8 88'  `88 88'`88'`88 88ooood8 88'  `88 \r\nY8.   .88 88.  ... 88.  .88 88  88  88 88.  ... 88.  .88 \r\n Y88888P' `88888P' `88888P' dP  dP  dP `88888P' `88888P8", ConsoleColor.Cyan);
app.Run();

// a88888b.                                             dP
//d8'   `88                                             88 
//88        .d8888b. .d8888b. 88d8b.d8b. .d8888b. .d888b88 
//88        88ooood8 88'  `88 88'`88'`88 88ooood8 88'  `88 
//Y8.   .88 88.  ... 88.  .88 88  88  88 88.  ... 88.  .88 
// Y88888P' `88888P' `88888P' dP  dP  dP `88888P' `88888P8