using Api.Bank.Domain.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Domain.Interfaces
{
    /// <summary>
    /// Interface do serviço de boleto bancário.
    /// </summary>
    public interface IBankSlipService
    {
        /// <summary>
        /// Método que obtém lista paginável de bancos.
        /// </summary>
        /// <param name="id">Id do boleto.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do boleto bancário.</returns>
        Task<BankSlipDTO> GetAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Método que registra um novo boleto bancário.
        /// </summary>
        /// <param name="request">Dados do boleto à ser registrado.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do boleto registrado.</returns>
        Task<BankSlipDTO> PostAsync([NotNull] BankSlipDTO request, CancellationToken cancellationToken = default);
    }
}
