using Api.Bank.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Bank.Infra.Data
{
    /// <summary>
    /// Classe de configurações da entidade.
    /// </summary>
    public class BankSlipConfigurations : IEntityTypeConfiguration<BankSlipEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<BankSlipEntity> builder)
        {
            builder.ToTable("BankSlips", "bnk");

            builder.HasKey(x => x.Id).HasName("PK_BANKSLIP_ID");
            builder.HasIndex(x => x.BankId).IsUnique(false).HasDatabaseName("IDX_BANKSLIP_BANK_KEY");

            builder.Property(x => x.PayerName).HasColumnType("VARCHAR").HasMaxLength(150).IsRequired();
            builder.Property(x => x.PayerDocument).HasColumnType("VARCHAR").HasMaxLength(14).IsRequired();
            builder.Property(x => x.BeneficiaryName).HasColumnType("VARCHAR").HasMaxLength(150).IsRequired();
            builder.Property(x => x.BeneficiaryDocument).HasColumnType("VARCHAR").HasMaxLength(14).IsRequired();
            builder.Property(x => x.Value).HasPrecision(10, 2).IsRequired();
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.Observations).HasColumnType("VARCHAR").HasMaxLength(200).IsRequired();
        }
    }
}
