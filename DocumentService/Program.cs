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

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document Service API", Version = "v1" });
    c.EnableAnnotations();
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// Регистрируем зависимости
builder.Services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
builder.Services.AddSingleton<JsonDocumentSerializer>();
builder.Services.AddSingleton<XmlDocumentSerializer>();
builder.Services.AddSingleton<MessagePackDocumentSerializer>();
builder.Services.AddSingleton<ISerializerFactory, SerializerFactory>();
builder.Services.AddSingleton<DocumentServiceManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Service API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
