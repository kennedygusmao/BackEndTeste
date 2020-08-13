
using HealthChecks.NpgSql;
using HealthChecks.UI.Client;
using Inlog.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Inlog.API.ApiConfiguration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var dadosDependencias = new List<Dependency>();

            new ConfigureFromConfigurationOptions<List<Dependency>>(
                configuration.GetSection("Dependencies"))
                    .Configure(dadosDependencias);
            dadosDependencias = dadosDependencias.OrderBy(d => d.Name).ToList();

            // Verificando a disponibilidade dos bancos de dados
            // da aplicação através de Health Checks
            services.AddHealthChecks()
                .AddDependencies(dadosDependencias);
            services.AddHealthChecksUI();

            //services.AddHealthChecks()
            //.AddNpgSql(
            //npgsqlConnectionString: configuration.GetConnectionString("DefaultConnection"),
            //healthQuery: "SELECT 1;",
            //name: "sql",
            //failureStatus: HealthStatus.Degraded,
            //tags: new string[] { "db", "sql", "postgress" });

            //services.AddHealthChecks()         
            //    .AddCheck("Veiculos", new NpgSqlHealthCheck(configuration.GetConnectionString("DefaultConnection"), "select 1"))
            //    .AddNpgSql(configuration.GetConnectionString("DefaultConnection"), "SELECT * from \"Veiculo\" ", name: "BancoPost");

          //  services.AddHealthChecksUI();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {


            //app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});
            //app.UseHealthChecksUI(options => { options.UIPath = "/api/hc-ui"; });

            // app.UseHealthChecksUI();

            // Gera o endpoint que retornará os dados utilizados no dashboard
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // Ativa o dashboard para a visualização da situação de cada Health Check
            app.UseHealthChecksUI();

            return app;
        }

      
    }



}
