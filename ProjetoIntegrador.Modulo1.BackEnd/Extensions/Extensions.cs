﻿using ProjetoIntegrador.Modulo1.BackEnd.Interfaces;
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
                .AddTransient<IProdutosService, ProdutosService>()
                .AddTransient<IProdutosRepository, ProdutosRepository>();
        }
    }
}