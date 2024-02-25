using Api.Bank.Application.Abstracts;
using Api.Bank.Application.Attributes;
using Api.Bank.Domain;
using Api.Bank.Domain.DTOs;
using Api.Bank.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Bank.Application.Controllers
{
    /// <summary>
    /// Classe de controle de endpoints para banco.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BanksController : AbstractController
    {
        private readonly IBankService _service;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="provider">Instância do provedor de serviços.</param>
        public BanksController(IServiceProvider provider) : base(provider)
        {
            this._service = this.GetService<IBankService>();
        }

        /// <summary>
        /// Endpoint para obtenção de lista paginável de bancos.
        /// </summary>
        /// <param name="skip">Itens à serem ignorados.</param>
        /// <param name="take">Itens à serem obtidos.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Lista paginável de bancos.</returns>
        [HttpGet]
        [Route("{skip}/{take}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<PageableCollectionDTO<BankDTO>>> GetAsync([FromRoute] int skip, [FromRoute] int take, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await this._service.GetAsync(skip, take, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        /// <summary>
        /// Endpoint para obtenção de dados de um banco.
        /// </summary>
        /// <param name="code">Código do banco.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do banco.</returns>
        [HttpGet]
        [Route("{code}")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<BankDTO>> GetAsync([FromRoute] string code, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await this._service.GetAsync(code, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return this.Error(ex);
            }
        }

        /// <summary>
        /// Endpoint para registro de um banco.
        /// </summary>
        /// <param name="request">Dados do banco.</param>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Dados do banco.</returns>
        [HttpPost]
        [Route("")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<BankDTO>> PostAsync([FromBody] BankDTO request, CancellationToken cancellationToken = default)
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
