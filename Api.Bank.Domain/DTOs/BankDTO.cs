namespace Api.Bank.Domain.DTOs
{
    /// <summary>
    /// DTO de banco.
    /// </summary>
    public class BankDTO
    {
        /// <summary>
        /// Id do registro.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public new virtual string ToString()
        {
            return $"[{this.Code}]";
        }
    }
}
