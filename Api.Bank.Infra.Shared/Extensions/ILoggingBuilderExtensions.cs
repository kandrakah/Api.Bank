using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Settings.Configuration;

namespace Api.Bank.Infra.Shared.Extensions
{
    /// <summary>
    /// Extensões para <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static class ILoggingBuilderExtensions
    {
        /// <summary>
        /// Extensão para adicionar o serviço Serilog.
        /// </summary>
        /// <param name="loggingBuilder">Objeto referenciado.</param>
        /// <param name="config">Instância do container de configurações.</param>
        /// <param name="sectionName">Nome da seção de configurações.</param>
        /// <returns>Objeto referenciado.</returns>
        public static ILoggingBuilder AddSerilog(this ILoggingBuilder loggingBuilder, IConfiguration config, string sectionName = "Serilog")
        {
            var loggerConfiguration = new LoggerConfiguration();
            if (config.GetSection(sectionName).Exists())
            {
                var options = new ConfigurationReaderOptions()
                {
                    SectionName = sectionName
                };

                loggerConfiguration.ReadFrom.Configuration(config, options);
            }
            else
            {
                Dictionary<string, string> settings = new Dictionary<string, string>
            {
                { "write-to:Console", "" },
                { "using:File", "Serilog.Sinks.Console" }
            };
                loggerConfiguration.ReadFrom.KeyValuePairs(settings);
            }

            var logger = loggerConfiguration.CreateLogger();
            loggingBuilder.ClearProviders();
            return loggingBuilder.AddSerilog(logger, dispose: true);
        }
    }
}
