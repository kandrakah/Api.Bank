using Microsoft.EntityFrameworkCore;

namespace Api.Bank.Domain.Interfaces
{
    /// <summary>
    /// Interface da unidade de trabalho.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWork<DbContext>
    {

    }

    /// <summary>
    /// Interface da unidade de trabalho.
    /// </summary>
    /// <typeparam name="TContext">Contexto à ser utilizado pela unidade de trabalho.</typeparam>
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        /// <summary>
        /// Método que cria uma instância de um <see cref="DbSet{TEntity}"/> de uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <returns>DbSet da entidade solicitada.</returns>
        DbSet<T> Set<T>() where T : AbstractEntity;

        /// <summary>
        /// Método que efetua a confirmação de alterações do contexto.
        /// </summary>
        /// <returns>Contagem de registros alterados.</returns>
        int SaveChanges();

        /// <summary>
        /// Método que efetua a confirmação de alterações do contexto.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Contagem de registros alterados.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
