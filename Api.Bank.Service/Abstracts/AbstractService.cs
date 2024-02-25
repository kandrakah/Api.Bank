using Api.Bank.Infra.Shared;
using Microsoft.Extensions.Logging;

namespace Api.Bank.Service.Abstracts
{
    /// <summary>
    /// Classe abstrata de serviço.
    /// </summary>
    public abstract class AbstractService : IDisposable
    {
        /// <summary>
        /// Instância do serviço de logs.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Instância do provedor de serviços da aplicação.
        /// </summary>
        protected readonly IServiceProvider _provider;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="provider">Instância do provedor de serviços da aplicação.</param>
        public AbstractService(IServiceProvider provider)
        {
            this._provider = provider;

            var loggerFactory = this._provider.GetInternalService<ILoggerFactory>();
            this._logger = loggerFactory.CreateLogger(this.GetType().Name);
        }

        /// <summary>
        /// Método que obtém a instância de um serviço.
        /// </summary>
        /// <typeparam name="T">Interface do serviço solicitada.</typeparam>
        /// <returns>Instância do serviço solicitado.</returns>
        protected internal T GetService<T>()
        {
            var result = this._provider.GetInternalService<T>();
            return result;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método para liberação de recursos usados pelo serviço.
        /// </summary>
        /// <param name="disposing">Define se propriedades filhas também devem ser liberadas.</param>
        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
