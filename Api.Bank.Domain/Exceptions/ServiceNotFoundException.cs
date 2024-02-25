using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Domain.Exceptions
{
    /// <summary>
    /// Classe de exceção de serviço não localizado.
    /// </summary>
    public class ServiceNotFoundException : ServiceException
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="message">Mensagem de exceção.</param>
        public ServiceNotFoundException([NotNull] string message) : base(System.Net.HttpStatusCode.InternalServerError, message)
        {
        }

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="type">Tipo do serviço.</param>
        public ServiceNotFoundException([NotNull] Type type) : base(System.Net.HttpStatusCode.InternalServerError, $"O serviço [{type?.Name}] não foi localizado.")
        {

        }

        /// <summary>
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
