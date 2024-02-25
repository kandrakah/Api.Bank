namespace Api.Bank.Domain
{
    /// <summary>
    /// DTO de paginação de itens.
    /// </summary>
    /// <typeparam name="T">Tipo de objeto da lista de itens.</typeparam>
    public class PageableCollectionDTO<T>
    {
        /// <summary>
        /// Itens da lista.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Quantidade total de itens.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public override string ToString()
        {
            var items = this.Items != null ? this.Items.Count() : 0;
            return $"{items}/{this.Total}";
        }
    }
}
