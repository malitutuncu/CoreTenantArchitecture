using Core.Global.Settings;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.Identity.Contexts
{
    public class IdentityContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly ConnectionSettings _connectionSettings;
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            //  _connectionSettings = connectionSettings.Value;
             
        }

 

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        //}
    }
}
