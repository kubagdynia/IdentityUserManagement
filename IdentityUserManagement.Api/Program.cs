using IdentityUserManagement.Api.Endpoints;
using IdentityUserManagement.Api.Extensions;
using IdentityUserManagement.Application.Extensions;
using IdentityUserManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(); // Use the global exception handler

app.UseAuthentication();
app.UseAuthorization();

app.MapAccountEndpoints();
app.MapTestEndpoints();

app.Run();
