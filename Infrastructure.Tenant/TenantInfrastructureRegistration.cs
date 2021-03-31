using Core.Global.Interfaces;
using Core.Global.Settings;
using Infrastructure.Tenant.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Tenant
{
    public static class TenantInfrastructureRegistration
    {
        public static void AddTenantInfrastructure(this IServiceCollection services, IConfiguration _configuration)
        {
            //var _connectionSettings = _configuration.GetSection(nameof(ConnectionSettings)) as ConnectionSettings;

            //var builder = new SqlConnectionStringBuilder
            //{
            //    UserID = _connectionSettings.UserId,
            //    Password = _connectionSettings.Password,
            //    InitialCatalog = _connectionSettings.TenantDbName + "1",
            //    DataSource = _connectionSettings.Server,
            //};

            //var builder = new SqlConnectionStringBuilder
            //{
            //    UserID = _configuration["ConnectionSettings:UserId"],
            //    Password = _configuration["ConnectionSettings:Password"],
            //    InitialCatalog = _configuration["ConnectionSettings:TenantDbName"] + "_3",
            //    DataSource = _configuration["ConnectionSettings:Server"],
            //};

            services.AddDbContext<TenantDbContext>();
            //(options =>
            //options.UseSqlServer(
            //    builder.ConnectionString,
            //    b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName)));

            //services.AddDbContext<TenantDbContext>((serviceProvider, dbContextBuilder) =>
            //{
            //    var connectionStringPlaceHolder = _configuration.GetConnectionString("PlaceHolderConnection");
            //    var authenticatedUser = serviceProvider.GetRequiredService<IAuthenticatedUserService>();


            //    var tenantId = 1;
            //    tenantId = authenticatedUser.TenantId;

            //    var connectionString = $"Server={_conn.Server};Database={_conn.TenantDbName}_{tenantId};User Id={_conn.UserId};Password={_conn.Password};";

            //    optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName));

            //    base.OnConfiguring(optionsBuilder);







            //    var dbName = httpContextAccessor.HttpContext.Request.Headers["tenantId"].First();
            //    var connectionString = connectionStringPlaceHolder.Replace("{dbName}", dbName);
            //    dbContextBuilder.UseSqlServer(connectionString);
            //});


        }
    }
}
