using IdentityUserManagement.Api.Endpoints;
using IdentityUserManagement.Api.ExceptionHandling;
using IdentityUserManagement.Application.Extensions;
using IdentityUserManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer() // Add the endpoint API explorer for generating OpenAPI document.
    .AddSwaggerGen(opt => opt.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(); // Use the global exception handler

app.MapAccountEndpoints();

app.Run();
