namespace Api.Bank.Domain.Entities
{
    /// <summary>
    /// Classe de entidade de banco.
    /// </summary>
    public class BankEntity : AbstractEntity
    {
        /// <summary>
        /// Código do banco.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Nome do banco.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Taxa de juros do banco.
        /// </summary>
        public decimal InterestIndex { get; set; }

        /// <summary>
        /// Relacionamento com boletos bancários.
        /// </summary>
        public IEnumerable<BankSlipEntity> BankSlips { get; set; }

        /// <summary>
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public new virtual string ToString()
        {
            return $"[{this.Code}]";
        }
    }
}
