using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Domain.Exceptions
{
    /// <summary>
    /// Classe de exceção para entidade não localizada.
    /// </summary>
    public class EntityNotFoundException : ServiceException
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="message">Mensagem de exceção.</param>
        public EntityNotFoundException([NotNull] string message) : base(System.Net.HttpStatusCode.NotFound, message)
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
