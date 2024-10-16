using IdentityUserManagement.Api.Endpoints;
using IdentityUserManagement.Application.Extensions;
using IdentityUserManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer() // Add the endpoint API explorer for generating OpenAPI document.
    .AddSwaggerGen(opt => opt.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAccountEndpoints();

app.Run();
