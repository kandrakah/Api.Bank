using Api.Bank.Domain;
using Api.Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Bank.Infra.Data.Implementations
{
    /// <summary>
    /// Implementação da unidade de trabalho.
    /// </summary>
    public class UnitOfWork : UnitOfWork<DbContext>, IUnitOfWork
    {
        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="context">Instância do contexto.</param>
        public UnitOfWork(DbContext context):base(context)
        {
        }        
    }

    /// <summary>
    /// Implementação da unidade de trabalho.
    /// </summary>
    /// <typeparam name="TContext">Contexto à ser utilizado pela unidade de trabalho.</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Instância do contexto.
        /// </summary>
        private TContext _context;

        /// <summary>
        /// Método construtor.
        /// </summary>
        /// <param name="context">Instância do contexto.</param>
        public UnitOfWork(TContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Método que cria uma instância de um <see cref="DbSet{TEntity}"/> de uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <returns>DbSet da entidade solicitada.</returns>
        public DbSet<T> Set<T>() where T : AbstractEntity
        {
            return this._context.Set<T>();
        }

        /// <summary>
        /// Método que efetua a confirmação de alterações do contexto.
        /// </summary>
        /// <returns>Contagem de registros alterados.</returns>
        public int SaveChanges()
        {
            var result = this._context.SaveChanges();
            return result;
        }

        /// <summary>
        /// Método que efetua a confirmação de alterações do contexto.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento de operação assíncrona.</param>
        /// <returns>Contagem de registros alterados.</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await this._context.SaveChangesAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Método que efetua a liberação de resursos de um objeto.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
