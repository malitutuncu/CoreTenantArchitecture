using Core.Global.DTOs.Authentication;
using Core.Global.Interfaces;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


       [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
           



            return Ok(await _userService.LoginAsync(request, GenerateIPAddress()));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.RegisterAsync(request, origin));
        }

        //[HttpGet("confirm-email")]
        //public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        //{
        //    var origin = Request.Headers["origin"];
        //    return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        //}

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        //{
        //    await _accountService.ForgotPassword(model, Request.Headers["origin"]);
        //    return Ok();
        //}

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        //{

        //    return Ok(await _accountService.ResetPassword(model));
        //}
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
