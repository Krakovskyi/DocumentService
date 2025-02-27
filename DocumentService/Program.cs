global using Microsoft.AspNetCore.Mvc;
global using System.Threading.Tasks;
global using DocumentService.Models;
global using DocumentService.Repositories;
global using DocumentService.Serializers;
global using DocumentService.Services;
global using Microsoft.Extensions.Primitives;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.Annotations;
global using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document Service API", Version = "v1" });
    
    // Включаем аннотации Swagger
    c.EnableAnnotations();
    
    // Включаем примеры
    c.ExampleFilters();
});

// Регистрируем примеры
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// Dependency Injection
// Register repositories
builder.Services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();

// Register serializers
builder.Services.AddSingleton<JsonDocumentSerializer>();
builder.Services.AddSingleton<XmlDocumentSerializer>();
builder.Services.AddSingleton<MessagePackDocumentSerializer>();
builder.Services.AddSingleton<ISerializerFactory, SerializerFactory>();

// Register DocumentServiceManager as Singleton for better performance
builder.Services.AddSingleton<DocumentServiceManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in development environment
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Service API v1"));
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Enable authorization
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Start the application
app.Run();
