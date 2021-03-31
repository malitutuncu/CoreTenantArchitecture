using Infrastructure.Tenant.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TenantDbContext _context;
        public ProductController(TenantDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var a = HttpContext.User;
            var claims = HttpContext.User.Claims;
            return Ok(await _context.Products.ToListAsync());
             
        }

        [HttpGet]
        public IActionResult GetClaim()
        {
            var claims = new List<Claim>();
            for (int i = 0; i < 600; i++)
            {
                var str = RandomString(6);
                claims.Add(new Claim(str, str));
            }

        var identity = new ClaimsIdentity(claims,"erp");
             
            HttpContext.User.AddIdentity(identity);

            //  var claims1 = HttpContext.User.Claims;

            var c = HttpContext.User.Identities.FirstOrDefault(x => x.AuthenticationType == "erp");

            return Ok(c.Claims.ToList().Select(x => new { x.Type, x.Value }));
        }




        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }




    }
}
