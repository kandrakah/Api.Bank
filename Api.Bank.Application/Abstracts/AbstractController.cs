using Api.Bank.Domain.Exceptions;
using Api.Bank.Domain.Models;
using Api.Bank.Infra.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Api.Bank.Application.Abstracts
{
    /// <summary>
    /// Classe abstrata de controle.
    /// </summary>
    public abstract class AbstractController : ControllerBase
    {
        /// <summary>
        /// Instância do serviço de logs.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Instância do provedor de serviços. Veja <see cref="IServiceProvider"/> para mais detalhes.
        /// </summary>
        protected readonly IServiceProvider _provider;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// /// <param name="provider">Instância do provedor de serviços.</param>
        public AbstractController(IServiceProvider provider)
        {
            this._provider = provider;

            var loggerFactory = this.GetService<ILoggerFactory>();
            this._logger = loggerFactory.CreateLogger(this.GetType().Name);
        }

        /// <summary>
        /// Método que obtém a instância de um serviço.
        /// </summary>
        /// <typeparam name="T">Interface do serviço solicitada.</typeparam>
        /// <returns>Instância do serviço solicitado.</returns>
        protected T GetService<T>()
        {
            try
            {
                var result = this._provider.GetInternalService<T>();
                return result;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Falha ao obter serviço: {msg}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Método que responte à requisição com um erro.
        /// </summary>
        /// <param name="exception">exceção à ser respondida à requisição.</param>
        /// <returns>Resposta à ser devolvida ao requerente da requisição. <see cref="ActionResult"/></returns>
        protected ActionResult Error([NotNull] Exception exception)
        {
            try
            {
                var service = this.GetService<IConfiguration>();

                if (exception is AggregateException && exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }

                if (exception is ServiceException)
                {
                    var ex = exception as ServiceException;
                    this._logger?.LogError(ex, ex.Message);

                    var data = new StatusCodeModel()
                    {
                        Status = (int)ex.StatusCode,
                        Title = ex.Message
                    };

                    return this.StatusCode(data.Status, data);
                }
                else
                {
                    this._logger?.LogError(exception, exception.Message);

                    var content = new StatusCodeModel
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Title = "Falha ao processar requisição."
                    };
                    return this.StatusCode(content.Status, content);
                }

            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, $"Falha ao processar exceção: {ex.Message}");
                return this.StatusCode((int)HttpStatusCode.InternalServerError, "Falha geral ao processar requisição.");
            }
        }
    }
}
