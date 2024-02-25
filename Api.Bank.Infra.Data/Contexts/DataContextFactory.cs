using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Api.Bank.Infra.Data
{
    /// <summary>
    /// Classe de fábrica de contexto.
    /// </summary>
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        /// <summary>
        /// Método que efetuará a criação do contexto.
        /// </summary>
        /// <param name="args">argumentos de criação.</param>
        /// <returns>Contexto criado.</returns>
        public DataContext CreateDbContext(string[] args)
        {

            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("appsettings.json", false);
            configBuilder.AddEnvironmentVariables();
            configBuilder.AddCommandLine(args);

            var configuration = configBuilder.Build();

            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("Default");
            var migrationsAssembly = typeof(DataContext).GetTypeInfo().Assembly.GetName().Name;

            builder.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly));

            var context = new DataContext(builder.Options);
            return context;
        }
    }
}
