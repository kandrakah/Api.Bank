using Api.Bank.Domain;
using Api.Bank.Domain.DTOs;
using Api.Bank.Domain.Entities;
using Api.Bank.Domain.Exceptions;
using Api.Bank.Domain.Interfaces;
using Api.Bank.Infra.Shared;
using Api.Bank.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Service
{
    /// <summary>
    /// Implementação do serviço de boleto bancário.
    /// </summary>
    public class BankSlipService : AbstractService, IBankSlipService
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="provider">Instância do provedor de serviços.</param>
        public BankSlipService(IServiceProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Método que obtém lista paginável de bancos.
        /// </summary>
        /// <param name="id">Id do boleto.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do boleto bancário.</returns>
        public async Task<BankSlipDTO> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now.Date;
            var result = default(BankSlipDTO);
            using (var unitOfWork = this.GetService<IUnitOfWork>())
            {
                result = await unitOfWork
                    .Set<BankSlipEntity>()
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new BankSlipDTO()
                    {
                        Id = x.Id,
                        BankId = x.BankId,
                        PayerName = x.PayerName,
                        PayerDocument = x.PayerDocument,
                        BeneficiaryName = x.BeneficiaryName,
                        BeneficiaryDocument = x.BeneficiaryDocument,
                        Value = x.Value,
                        DueDate = x.DueDate
                    }).FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                {
                    throw new EntityNotFoundException("O boleto solicitado não foi localizado.");
                }

                if (result.DueDate < now)
                {
                    var bankInterest = await unitOfWork
                        .Set<BankEntity>()
                        .AsNoTracking()
                        .Where(x => x.Id == result.BankId)
                        .Select(x => x.InterestIndex)
                        .FirstOrDefaultAsync(cancellationToken);

                    result.Value += result.Value * bankInterest;
                }
            }

            return result;
        }

        /// <summary>
        /// Método que registra um novo boleto bancário.
        /// </summary>
        /// <param name="request">Dados do boleto à ser registrado.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do boleto registrado.</returns>
        public async Task<BankSlipDTO> PostAsync([NotNull] BankSlipDTO request, CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = this.GetService<IUnitOfWork>())
            {
                await this.ValidateRequestAsync(unitOfWork, request, cancellationToken);

                var bankSlip = new BankSlipEntity()
                {
                    BankId = request.BankId,
                    PayerName = request.PayerName,
                    PayerDocument = request.PayerDocument.Unformat(),
                    BeneficiaryName = request.BeneficiaryName,
                    BeneficiaryDocument = request.BeneficiaryDocument.Unformat(),
                    Value = request.Value,
                    DueDate = request.DueDate,
                    Observations = request.Observations,
                };

                await unitOfWork
                    .Set<BankSlipEntity>()
                    .AddAsync(bankSlip, cancellationToken);

                request.Id = bankSlip.Id;

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return request;
        }

        /// <summary>
        /// Método que efetua a validação da requisição.
        /// </summary>
        /// <typeparam name="TContext">Tipo de contexto de dados.</typeparam>
        /// <param name="unitOfWork">Instância da unidade de trabalho.</param>
        /// <param name="request">Dados da requisição.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        private async Task ValidateRequestAsync<TContext>(IUnitOfWork<TContext> unitOfWork, [NotNull] BankSlipDTO request, CancellationToken cancellationToken = default) where TContext : DbContext
        {
            var now = DateTime.Now.Date;
            if (request == null)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "Requisição nula ou inválida.");
            };

            var bankExists = await unitOfWork
                .Set<BankEntity>()
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.BankId);

            if (!bankExists)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O banco informado não foi localizado.");
            }

            if (string.IsNullOrEmpty(request.PayerName))
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O nome do pagador é requerido.");
            }

            if (!request.PayerDocument.IsCpfOrCnpj())
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O número de documento do pagador é requerido.");
            }

            if (string.IsNullOrEmpty(request.BeneficiaryName))
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O nome do beneficiário é requerido.");
            }

            if (!request.BeneficiaryDocument.IsCpfOrCnpj())
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O número de do beneficiário do pagador é requerido.");
            }

            if (request.Value <= 0)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O valor do boleto informado é inválido.");
            }

            if (request.DueDate.Date < now)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "A data de vencimento do boleto informada é inválida.");
            }
        }
    }
}
