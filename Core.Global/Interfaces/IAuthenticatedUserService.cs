using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Global.Interfaces
{
    public interface IAuthenticatedUserService
    {
        int UserId { get; }

        string UserName { get; }

        int TenantId { get; }

    }
}
