using Api.Bank.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Bank.Infra.Shared
{
    /// <summary>
    /// Extensões para <see cref="IServiceProvider"/>.
    /// </summary>
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Extensão para obter um serviço.
        /// </summary>
        /// <typeparam name="T">Tipo do serviço requerido.</typeparam>
        /// <param name="provider">Objeto referenciado.</param>
        /// <returns>Objeto referenciado.</returns>
        public static T GetInternalService<T>(this IServiceProvider provider)
        {
            return provider.GetService<T>() ?? throw new ServiceNotFoundException(typeof(T));
        }
    }
}
