using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Api.Bank.Domain.Exceptions
{
    /// <summary>
    /// Classe de exceção de serviços de Api.
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Código de status de Exceção.
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        /// Método construtor.
        /// </summary>
        public ServiceException() : base(HttpStatusCode.InternalServerError.ToString())
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="statusCode">Código de status.</param>
        public ServiceException(HttpStatusCode statusCode) : base(statusCode.ToString())
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="statusCode">Código de status.</param>
        /// <param name="message">Mensagem destatus.</param>
        public ServiceException(HttpStatusCode statusCode, [NotNull] string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public override string ToString()
        {
            return $"[{(int)this.StatusCode}]{this.Message}";
        }
    }
}
