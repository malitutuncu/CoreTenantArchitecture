using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Global.Settings
{
    public class ConnectionSettings
    {
        public string Server { get; set; }
        public string IdentityDbName { get; set; }
        public string TenantDbName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string MigrationTenantId { get; set; }
    }
}
