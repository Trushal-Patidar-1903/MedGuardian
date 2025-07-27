using Configurations.Filters;
using DTO.Common.Request;
using DTO.Common.Response;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register the strongly-typed configuration
builder.Services.Configure<FileStorageSettings>(
    builder.Configuration.GetSection("FileStorageSettings")
);

// Add services to the container.
Repositories.Register.RepositoryRegister.AddRepository(builder.Services);
Services.Register.ServiceRegister.AddService(builder.Services);
Helper.SharedResource.Register.SharedResourceRegister.AddSharedResources(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.msS/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MedGuardian Web API",
        Version = "v1",
        Description = "AI-Powered Emergency Medical Info & Consent System",
        Contact = new OpenApiContact
        {
            Name = "Trushal Patel",
            Email = "trushalpatidar1903@gmail.com"
        }
    });

    // Define global response model for Swagger
    options.MapType<GlobalResponseModel<object>>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema> {
            { "status", new OpenApiSchema { Type = "string" } },
            { "statusCode", new OpenApiSchema { Type = "integer", Format = "int32" } },
            { "message", new OpenApiSchema { Type = "string" } },
            { "data", new OpenApiSchema { Type = "object" } }
        }
    });

    // Add global response filter
    options.OperationFilter<AddGlobalResponseOperationFilter>();
    options.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Developer exception page (only in development)
app.UseDeveloperExceptionPage();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();

// Custom middleware (if any)
//app.UseMiddlewareExtension();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
