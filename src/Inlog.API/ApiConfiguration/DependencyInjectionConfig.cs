using Inlog.Data.Context;
using Inlog.Data.Repository;
using Inlog.Domain.Interfaces.Repository;
using Inlog.Domain.Interfaces.Service;
using Inlog.Service.Interface;
using Inlog.Service.Notificacoes;
using Inlog.Service.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Inlog.API.ApiConfiguration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<InlogDbContext>();

            ///Repository
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();

            ///Service
            services.AddScoped<IVeiculoService, VeiculoService>();


            services.AddScoped<INotificador, Notificador>();         
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();



            return services;
        }
    }
}
