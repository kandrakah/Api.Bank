﻿// <auto-generated />
using System;
using Api.Bank.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Bank.Infra.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Api.Bank.Domain.BankSlipEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BankId")
                        .HasColumnType("uuid");

                    b.Property<string>("BeneficiaryDocument")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("BeneficiaryName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Observations")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("PayerDocument")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("PayerName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<decimal>("Value")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)");

                    b.HasKey("Id")
                        .HasName("PK_BANKSLIP_ID");

                    b.HasIndex("BankId")
                        .HasDatabaseName("IDX_BANKSLIP_BANK_KEY");

                    b.ToTable("BankSlips", "bnk");
                });

            modelBuilder.Entity("Api.Bank.Domain.Entities.BankEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("VARCHAR");

                    b.Property<decimal>("InterestIndex")
                        .HasPrecision(10, 5)
                        .HasColumnType("numeric(10,5)");

                    b.HasKey("Id")
                        .HasName("PK_BANK_ID");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("IDX_BANK_KEY");

                    b.ToTable("Banks", "bnk");
                });

            modelBuilder.Entity("Api.Bank.Domain.BankSlipEntity", b =>
                {
                    b.HasOne("Api.Bank.Domain.Entities.BankEntity", "Bank")
                        .WithMany("BankSlips")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_BANK_BANKSLIPS");

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Api.Bank.Domain.Entities.BankEntity", b =>
                {
                    b.Navigation("BankSlips");
                });
#pragma warning restore 612, 618
        }
    }
}
