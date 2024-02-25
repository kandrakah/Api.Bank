using Api.Bank.Application.Abstracts;
using Api.Bank.Application.Attributes;
using Api.Bank.Domain.DTOs;
using Api.Bank.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Bank.Application.Controllers
{
    /// <summary>
    /// Classe de controle de endpoints para boleto bancário.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BankSlipsController : AbstractController
    {
        private readonly IBankSlipService _service;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="provider">Instância do provedor de serviços.</param>
        public BankSlipsController(IServiceProvider provider) : base(provider)
        {
            this._service = this.GetService<IBankSlipService>();
        }

        /// <summary>
        /// Endpoint  que obtém lista paginável de bancos.
        /// </summary>
        /// <param name="id">Id do boleto.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do boleto bancário.</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<BankSlipDTO>> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await this._service.GetAsync(id, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        /// <summary>
        /// Endpoint que registra um novo boleto bancário.
        /// </summary>
        /// <param name="request">Dados do boleto à ser registrado.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>DDados do boleto registrado.</returns>
        [HttpPost]
        [Route("")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<BankSlipDTO>> PostAsync([FromBody] BankSlipDTO request, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await this._service.PostAsync(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }
    }
}
