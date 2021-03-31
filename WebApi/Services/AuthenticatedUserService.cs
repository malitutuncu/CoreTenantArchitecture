using Core.Global.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Core.Global.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor )
        {
            var aa = httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated;
            var a = httpContextAccessor.HttpContext?.User?.FindFirstValue("userid");
            UserId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimNames.UserId));
            TenantId = Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimNames.TenantId));
        }

        public int UserId { get; }

        public string UserName { get; }

        public int TenantId { get; }
    }
}
