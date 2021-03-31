using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.AuthenticatedService
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {

            UserId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == "user_id").Value);
            UserName = httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == "user_name").Value;
        }

        public int UserId { get; }

        public string UserName { get; }


    }
}
