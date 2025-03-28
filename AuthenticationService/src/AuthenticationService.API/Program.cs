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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//TODO Infrastructure replace as IndentityExtensions
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>(); // Register specific repository
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>(); // Register specific repository

builder.Services.AddApplication();
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
    await app.SeedRolesAndAdminAsync();
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
//CORS
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
