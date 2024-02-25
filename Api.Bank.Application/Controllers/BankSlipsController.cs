using Api.Bank.Application.Abstracts;
using Api.Bank.Application.Attributes;
using Api.Bank.Domain.DTOs;
using Api.Bank.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Bank.Application.Controllers
{
    /// <summary>
    /// Classe de controle de endpoints para boleto banc�rio.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BankSlipsController : AbstractController
    {
        private readonly IBankSlipService _service;

        /// <summary>
        /// M�todo construtor.
        /// </summary>
        /// <param name="provider">Inst�ncia do provedor de servi�os.</param>
        public BankSlipsController(IServiceProvider provider) : base(provider)
        {
            this._service = this.GetService<IBankSlipService>();
        }

        /// <summary>
        /// Endpoint  que obt�m lista pagin�vel de bancos.
        /// </summary>
        /// <param name="id">Id do boleto.</param>
        /// <param name="cancellationToken">Token de cancelamento de opera��o ass�ncrona.</param>
        /// <returns>Dados do boleto banc�rio.</returns>
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
        /// Endpoint que registra um novo boleto banc�rio.
        /// </summary>
        /// <param name="request">Dados do boleto � ser registrado.</param>
        /// <param name="cancellationToken">Token de cancelamento de opera��o ass�ncrona.</param>
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
