using Api.Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Bank.Infra.Data
{
    /// <summary>
    /// Classe de configurações da entidade.
    /// </summary>
    public class BankConfigurations : IEntityTypeConfiguration<BankEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<BankEntity> builder)
        {
            builder.ToTable("Banks", "bnk");

            builder.HasKey(x => x.Id).HasName("PK_BANK_ID");
            builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("IDX_BANK_KEY");

            builder.Property(x => x.Code).HasColumnType("VARCHAR").HasMaxLength(3).IsRequired();
            builder.Property(x => x.DisplayName).HasColumnType("VARCHAR").HasMaxLength(25).IsRequired();
            builder.Property(x => x.InterestIndex).HasPrecision(10, 5).IsRequired();

            builder.HasMany(x => x.BankSlips).WithOne(x => x.Bank).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_BANK_BANKSLIPS");
        }
    }
}
