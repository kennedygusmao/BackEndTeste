using Inlog.API.ApiConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;


namespace Inlog.API.Extensions
{
    public static class DependenciesExtensions
    {
        public static IHealthChecksBuilder AddDependencies(
            this IHealthChecksBuilder builder,
            List<Dependency> dependencies)
        {
            foreach (var dependencia in dependencies)
            {
                string nomeDependencia = dependencia.Name.ToLower();

                if (nomeDependencia.StartsWith("postgres-"))
                {
                    builder = builder.AddNpgSql(dependencia.ConnectionString, name: dependencia.Name);
                }
              
            }

            return builder;
        }
    }
}
