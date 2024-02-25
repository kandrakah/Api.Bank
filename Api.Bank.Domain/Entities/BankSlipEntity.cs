using Api.Bank.Domain.Entities;

namespace Api.Bank.Domain
{
    /// <summary>
    /// Classe de entidade de boleto bancário.
    /// </summary>
    public class BankSlipEntity : AbstractEntity
    {
        /// <summary>
        /// Id do banco.
        /// </summary>
        public Guid BankId { get; set; }

        /// <summary>
        /// Nome do pagador.
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// Número de documento do pagador.
        /// </summary>
        public string PayerDocument { get; set; }

        /// <summary>
        /// Nome do beneficiário.
        /// </summary>
        public string BeneficiaryName { get; set; }

        /// <summary>
        /// Número de documento do beneficiário.
        /// </summary>
        public string BeneficiaryDocument { get; set; }

        /// <summary>
        /// Valor do boleto.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Data de vencimento do boleto.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Observações do boleto.
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// Relacionamento com Banco.
        /// </summary>
        public virtual BankEntity Bank { get; set; }

        /// <inheritdoc/>
        public new virtual string ToString()
        {
            return $"[{this.PayerName}]";
        }
    }
}
