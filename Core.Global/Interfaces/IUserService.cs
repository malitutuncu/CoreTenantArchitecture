using Core.Global.Concrete;
using Core.Global.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Global.Interfaces
{
    public interface IUserService
    {
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request, string ipAddress);
        Task<Response<int>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<int>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
    }
}
