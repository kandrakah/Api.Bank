using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Bank.Domain
{
    /// <summary>
    /// Classe abstrata que serve de base para todas as entidades do sistema.
    /// </summary>
    public abstract class AbstractEntity
    {
        /// <summary>
        /// Índice único da entidade.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Método que traz uma cadeia de caracteres que representa o objeto atual.
        /// </summary>
        /// <returns>Cadeia de caracteres que representa o objeto atual.</returns>
        public new virtual string ToString()
        {
            return this.Id.ToString();
        }
    }
}
