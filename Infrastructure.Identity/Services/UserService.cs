using Core.Global.Concrete;
using Core.Global.Constants;
using Core.Global.DTOs.Authentication;
using Core.Global.DTOs.Email;
using Core.Global.Exceptions;
using Core.Global.Interfaces;
using Core.Global.Settings;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;

        public UserService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IOptions<JWTSettings> jwtSettings,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                // throw new ApiException($"No Accounts Registered with {request.Email}.");
            }
             
             
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                //throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }

            if (!user.EmailConfirmed)
            {
                //throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, request.TenantId);
            LoginResponse response = new LoginResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;



            return new Response<LoginResponse>(response, $"Authenticated {user.UserName}");

        }

        public async Task<Response<int>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                // throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            var user = new User
            {
                Email = request.Email,
                // FirstName = request.FirstName,
                // LastName = request.LastName,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ApiException(result.Errors.ToString());
            }

            return new Response<int>(user.Id);

            //var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            //if (userWithSameEmail == null)
            //{
            //    var result = await _userManager.CreateAsync(user, request.Password);
            //    if (result.Succeeded)
            //    {
            //        await _userManager.AddToRoleAsync(user, "rol kaydedecek");
            //        var verificationUri = await SendVerificationEmail(user, origin);
            //        //TODO: Attach Email Service here and configure it via appsettings
            //        await _emailService.SendAsync(new EmailRequest()
            //        { 
            //            From = "mail@codewithmukesh.com", 
            //            To = user.Email, 
            //            Body = $"Please confirm your account by visiting this URL {verificationUri}", 
            //            Subject = "Confirm Registration" 
            //        });
            //        return new Response<int>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
            //    }
            //    else
            //    {
            //        throw new Exception("aaa");
            //       // throw new ApiException($"{result.Errors}");
            //    }
            //}
            //else
            //{
            //    throw new Exception("aaa");
            //    //throw new ApiException($"Email {request.Email } is already registered.");
            //}
        }

        public async Task<Response<int>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<int>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new Exception("aa");
                //throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }


        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user, int tenantId)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < 750; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
                //var str = RandomString(6);
                //roleClaims.Add(new Claim(str, str));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimNames.UserId, user.Id.ToString()),
                new Claim(ClaimNames.IpAdress, ipAddress),
                new Claim(ClaimNames.TenantId, tenantId.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(User user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
    }
}
