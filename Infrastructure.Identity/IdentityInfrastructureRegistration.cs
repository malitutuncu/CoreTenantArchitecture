using Core.Global.Concrete;
using Core.Global.Interfaces;
using Core.Global.Settings;
using Infrastructure.Identity.Contexts;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Infrastructure.Identity
{
    public static class IdentityInfrastructureRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration _configuration)
        {
            // var _connectionSettings = _configuration.GetSection("ConnectionSettings") as ConnectionSettings;


            //var builder = new SqlConnectionStringBuilder
            //{
            //    UserID = _connectionSettings.UserId,
            //    Password = _connectionSettings.Password,
            //    InitialCatalog = _connectionSettings.IdentityDbName,
            //    DataSource = _connectionSettings.Server,
            //};

 
            var builder = new SqlConnectionStringBuilder
            {
                UserID = _configuration["ConnectionSettings:UserId"],
                Password = _configuration["ConnectionSettings:Password"],
                InitialCatalog = _configuration["ConnectionSettings:IdentityDbName"],
                DataSource = _configuration["ConnectionSettings:Server"],
            };

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    builder.ConnectionString,
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders();

            services.AddTransient<IUserService, UserService>();

            services.Configure<ConnectionSettings>(_configuration.GetSection("ConnectionSettings"));
            services.Configure<JWTSettings>(_configuration.GetSection("JWTSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = _configuration["JWTSettings:Issuer"],
                       ValidAudience = _configuration["JWTSettings:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]))
                   };
                   o.Events = new JwtBearerEvents()
                   {
                       OnAuthenticationFailed = c =>
                       {
                           c.NoResult();
                           c.Response.StatusCode = 500;
                           c.Response.ContentType = "text/plain";
                           return c.Response.WriteAsync(c.Exception.ToString());
                       },
                       OnChallenge = context =>
                       {
                           context.HandleResponse();
                           context.Response.StatusCode = 401;
                           context.Response.ContentType = "application/json";
                           var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                           return context.Response.WriteAsync(result);
                       },
                       OnForbidden = context =>
                       {
                           context.Response.StatusCode = 403;
                           context.Response.ContentType = "application/json";
                           var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                           return context.Response.WriteAsync(result);
                       },
                   };
               });

            //services.Configure<IdentityOptions>(options =>
            //{
            //    // password
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequiredLength = 3;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;

            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            //    options.Lockout.AllowedForNewUsers = false;

            //    // options.User.AllowedUserNameCharacters = "";
            //    options.User.RequireUniqueEmail = true;

            //    options.SignIn.RequireConfirmedEmail = false;
            //    options.SignIn.RequireConfirmedPhoneNumber = false;
            //});


        }
    }
}
