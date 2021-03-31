using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.AuthenticatedService
{
    public interface IAuthenticatedUserService
    {
        int UserId { get; }

        string UserName { get; }
    }
}
