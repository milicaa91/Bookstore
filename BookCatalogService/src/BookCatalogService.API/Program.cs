using BookCatalogService.API.Extensions;
using BookCatalogService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using BookCatalogService.Application.Extensions;
using Common.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCorsApp();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
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
