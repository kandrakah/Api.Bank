using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Bank.Infra.Data.Extensions
{
    /// <summary>
    /// Extensões para <see cref="IApplicationBuilder"/>
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Extensão que efetua a migração de forma automática do contexto.
        /// </summary>
        /// <typeparam name="TContext">Tipo do contexto de dados.</typeparam>
        /// <param name="builder">Objeto referenciado.</param>
        public static void ApplyMigrations<TContext>(this IApplicationBuilder builder) where TContext : DbContext
        {
            using (IServiceScope scope = builder.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ILogger<IApplicationBuilder>>().LogInformation("Efetuando migração do contexto {context}", typeof(TContext).Name);
                TContext requiredService = scope.ServiceProvider.GetRequiredService<TContext>();
                if (requiredService.Database.IsRelational())
                {
                    requiredService.Database.MigrateAsync().Wait();
                }
            }
        }
    }
}
