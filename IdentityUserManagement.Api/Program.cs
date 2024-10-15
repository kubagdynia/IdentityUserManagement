using IdentityUserManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer() // Add the endpoint API explorer for generating OpenAPI document.
    .AddSwaggerGen(opt => opt.CustomSchemaIds(t => t.FullName?.Replace('+', '.'))); // Add Swagger generator

builder.Services.AddInfrastructure(builder.Configuration); // Add the infrastructure services

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwaggerUI(); // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.

}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS.

app.MapGet("/test", (string name) => $"Hello, {name}!"); // Map the GET request to the path "/test" to the lambda function that returns a string.

app.Run();
