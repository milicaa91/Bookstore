using AuthenticationService.API.Extensions;
using AuthenticationService.Application.Extensions;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Interfaces.Repositories;
using AuthenticationService.Application.Interfaces.Services.TokenGeneratorService;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure;
using AuthenticationService.Infrastructure.Extensions;
using AuthenticationService.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Services.TokenGenerator;
using Common.Interfaces;
using Common.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCorsApp();
builder.Services.AddApplication();
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
    await app.SeedRolesAndAdminAsync();
    app.UseCors("AllowAll");
}
else
    app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
