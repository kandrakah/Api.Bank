using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Api.Bank.Infra.Data.Extensions
{
    /// <summary>
    /// Extensões para <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Extensão para adição do Postgres como base de dados.
        /// </summary>
        /// <param name="builder">Objeto referenciado.</param>
        /// <param name="connectionString">String de conexão.</param>
        /// <returns>Objeto referenciado.</returns>
        public static DbContextOptionsBuilder UsePostgres([NotNull] this DbContextOptionsBuilder builder, [NotNull] string connectionString)
        {
            builder.UseNpgsql(connectionString);
            return builder;
        }
    }
}
