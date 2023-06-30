using Microsoft.AspNetCore.Authentication.Cookies;
using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
using ProjetoIntegrador.Modulo1.BackEnd.Models;
using ProjetoIntegrador.Modulo1.BackEnd.Repositorios;
using ProjetoIntegrador.Modulo1.BackEnd.Servicos;

namespace ProjetoIntegrador.Modulo1.BackEnd.Extensions
{
    public static class Extensions
    {
        public static void ConfigureAppSettings(this WebApplicationBuilder builder)
        {
            IConfigurationRoot implementationInstance = builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            builder.Services.AddSingleton(implementationInstance);
        }
        
        public static void AddConnectionString(this WebApplicationBuilder builder)
        {
            DB dbInfo = builder.Configuration.GetSection("DB").Get<DB>();
            builder.Services.AddSingleton(dbInfo);
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<IProdutosService, ProdutosService>()
                .AddTransient<IPagamentosService, PagamentosService>()
                .AddTransient<IAdminRepository, AdminRepository>()
                .AddTransient<IProdutosRepository, ProdutosRepository>()
                .AddTransient<IPagamentosRepository, PagamentosRepository>();
        }

        public static string AllowCors(this WebApplicationBuilder builder)
        {
            var policyName = "AllowAll";
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return policyName;
        }

        public static void AddCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "AdminSession";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };
                });
        }
    }
}
