using Api.Bank.Domain.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Domain.Interfaces
{
    /// <summary>
    /// Interface do serviço de banco.
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Método que obtém lista paginável de bancos.
        /// </summary>
        /// <param name="skip">Itens à serem ignorados.</param>
        /// <param name="take">Itens à serem obtidos.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Coleção paginada de bancos.</returns>
        Task<PageableCollectionDTO<BankDTO>> GetAsync(int skip, int take, CancellationToken cancellationToken = default);

        /// <summary>
        /// Método que obtém lista paginável de bancos.
        /// </summary>
        /// <param name="code">Código do banco.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do banco.</returns>
        Task<BankDTO> GetAsync([NotNull] string code, CancellationToken cancellationToken = default);

        /// <summary>
        /// Métoodo que registra um banco.
        /// </summary>
        /// <param name="request">Dados do banco.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do banco registrado.</returns>
        Task<BankDTO> PostAsync([NotNull] BankDTO request, CancellationToken cancellationToken = default);
    }
}
