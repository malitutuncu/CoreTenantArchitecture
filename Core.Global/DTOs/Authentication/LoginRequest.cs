using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Global.DTOs.Authentication
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int TenantId { get; set; }
    }
}
