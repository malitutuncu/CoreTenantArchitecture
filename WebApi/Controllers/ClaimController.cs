using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetClaim2()
        {
            var c = HttpContext.User.Identities.FirstOrDefault(x => x.AuthenticationType == "erp");

            return Ok(c.Claims.ToList().Select(x => new { x.Type, x.Value }));
        }

        [HttpGet]
        public IActionResult GetClaim3()
        {
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            return Ok(claims);
        }
    }
}
