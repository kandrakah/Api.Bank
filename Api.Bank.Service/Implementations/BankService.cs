using Api.Bank.Domain;
using Api.Bank.Domain.DTOs;
using Api.Bank.Domain.Entities;
using Api.Bank.Domain.Exceptions;
using Api.Bank.Domain.Interfaces;
using Api.Bank.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Service
{
    /// <summary>
    /// Implementação do serviço de banco.
    /// </summary>
    public class BankService : AbstractService, IBankService
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="provider">Instância do provedor de serviços.</param>
        public BankService(IServiceProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Método que obtém lista paginável de bancos.
        /// </summary>
        /// <param name="skip">Itens à serem ignorados.</param>
        /// <param name="take">Itens à serem obtidos.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Coleção paginada de bancos.</returns>
        public async Task<PageableCollectionDTO<BankDTO>> GetAsync(int skip, int take, CancellationToken cancellationToken = default)
        {
            var result = new PageableCollectionDTO<BankDTO>();

            using (var unitOfWork = this.GetService<IUnitOfWork>())
            {
                var bankQuery = unitOfWork
                    .Set<BankEntity>()
                    .AsNoTracking()
                    .AsQueryable();

                result.Total = await bankQuery.CountAsync(cancellationToken);

                result.Items = await bankQuery
                    .OrderBy(x => x.Code)
                    .Select(x => new BankDTO()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        DisplayName = x.DisplayName,
                        InterestIndex = x.InterestIndex
                    })
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }

            return result;
        }

        /// <summary>
        /// Método que obtém os dados de um banco.
        /// </summary>
        /// <param name="code">Código do banco.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do banco.</returns>
        public async Task<BankDTO> GetAsync([NotNull] string code, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O código do banco é requerido.");
            }

            var result = default(BankDTO);
            using (var unitOfWork = this.GetService<IUnitOfWork>())
            {
                result = await unitOfWork
                    .Set<BankEntity>()
                    .AsNoTracking()
                    .Where(x => x.Code == code)
                    .Select(x => new BankDTO()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        DisplayName = x.DisplayName,
                        InterestIndex = x.InterestIndex
                    }).FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                {
                    throw new EntityNotFoundException("O banco solicitado não foi localizado.");
                }
            }

            return result;
        }

        /// <summary>
        /// Métoodo que registra um banco.
        /// </summary>
        /// <param name="request">Dados do banco.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do banco registrado.</returns>
        public async Task<BankDTO> PostAsync([NotNull] BankDTO request, CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = this.GetService<IUnitOfWork>())
            {
                await this.ValidateRequestAsync(unitOfWork, request, cancellationToken);

                var bank = new BankEntity()
                {
                    Code = request.Code,
                    DisplayName = request.DisplayName,
                    InterestIndex = request.InterestIndex
                };

                await unitOfWork
                    .Set<BankEntity>()
                    .AddAsync(bank, cancellationToken);

                request.Id = bank.Id;

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
        private async Task ValidateRequestAsync<TContext>(IUnitOfWork<TContext> unitOfWork, BankDTO request, CancellationToken cancellationToken) where TContext : DbContext
        {
            var now = DateTime.Now.Date;
            if (request == null)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "Requisição nula ou inválida.");
            };


            if (string.IsNullOrEmpty(request.Code))
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O código do banco é requerido.");
            }
            else if (request.Code.Length != 3)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O código do banco é inválido.");
            }

            request.Code = request.Code.PadLeft(3, '0');

            var codeExists = await unitOfWork
                .Set<BankEntity>()
                .AsNoTracking()
                .AnyAsync(x => x.Code == request.Code);

            if (codeExists)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "código do banco já está cadastrado.");
            }

            if (string.IsNullOrEmpty(request.DisplayName))
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "O nome do banco é requerido.");
            }

            if (request.InterestIndex < 0)
            {
                throw new ServiceException(System.Net.HttpStatusCode.BadRequest, "A taxa de juros deve ser positiva.");
            }
        }
    }
}
