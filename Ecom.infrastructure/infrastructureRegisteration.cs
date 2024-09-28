using Ecom.Core.Entities;
using Ecom.Core.interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositries;
using Ecom.infrastructure.Repositries.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
namespace Ecom.infrastructure;

public static class infrastructureRegisteration
{
    public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
        //apply Unit OF Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //register email sender
        services.AddScoped<IEmailService, EmailService>();

        //register token
        services.AddScoped<IGenerateToken, GenerateToken>();
        //apply Redis Connectoon
        services.AddSingleton<IConnectionMultiplexer>(i =>
        {
            var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
            return ConnectionMultiplexer.Connect(config);
        });

        services.AddSingleton<IImageManagementService, ImageManagementService>();
        services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        //apply DbContext
        services.AddDbContext<AppDbContext>(op =>
        {
            op.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
        });

        services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(op =>
          {
              op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          })

          .AddCookie(o =>
          {
              o.Cookie.Name = "token";
              o.Events.OnRedirectToLogin = context =>
              {
                  context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                  return Task.CompletedTask;
              };
          })

          .AddJwtBearer(op =>
          {
              op.RequireHttpsMetadata = false;
              op.SaveToken = true;
              op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Secret"])),
                  ValidateIssuer = true,
                  ValidIssuer = configuration["Tokne:Issure"],
                  ValidateAudience = false,
                  ClockSkew = TimeSpan.Zero,
              };
              op.Events = new JwtBearerEvents()
              {
                  OnMessageReceived = context =>
                  {
                      context.Token = context.Request.Cookies["token"];
                      return Task.CompletedTask;
                  }
              };
          });
        return services;
    }
}
