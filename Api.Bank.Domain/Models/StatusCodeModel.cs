namespace Api.Bank.Domain.Models
{
    /// <summary>
    /// Modelo de retorno de dados em caso de falhas de requisição.
    /// </summary>
    public class StatusCodeModel
    {
        /// <summary>
        /// Código de status da falha.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Descrição da falha.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public override string ToString()
        {
            return this.Title;
        }
    }
}
