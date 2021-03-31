using Core.Global.Infrastructure;
using Core.Global.Interfaces;
using Core.Global.Settings;
using Infrastructure.Tenant.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Tenant.Context
{
    public class TenantDbContext : DbContext
    {
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly ConnectionSettings _conn;

        public TenantDbContext(
            DbContextOptions<TenantDbContext> options,
            IAuthenticatedUserService authenticatedUser,
            IOptions<ConnectionSettings> connectionSettings) : base(options)
        {
            _authenticatedUser = authenticatedUser;
            _conn = connectionSettings.Value;
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            var tenantId = 1;
            //tenantId = _authenticatedUser.TenantId;

            var connectionString = $"Server={_conn.Server};Database={_conn.TenantDbName}_{tenantId};User Id={_conn.UserId};Password={_conn.Password};";

            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName));

            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IModifiableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _authenticatedUser.UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserName;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}