using Core.Global.Interfaces;
using Core.Global.Settings;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Shared
{
    public static class SharedInfrastructureRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _configuration)
        {
            services.Configure<MailSettings>(_configuration.GetSection(nameof(MailSettings)));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
