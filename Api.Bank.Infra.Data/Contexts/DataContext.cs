using Microsoft.EntityFrameworkCore;

namespace Api.Bank.Infra.Data
{
    /// <summary>
    /// Implementação do contexto de dados.
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="options">Opções do contexto.</param>
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Método que faz a criação dos modelos do contexto.
        /// </summary>
        /// <param name="modelBuilder">Instância do contrutor de modelos do contexto.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BankConfigurations());
            modelBuilder.ApplyConfiguration(new BankSlipConfigurations());
        }
    }
}
