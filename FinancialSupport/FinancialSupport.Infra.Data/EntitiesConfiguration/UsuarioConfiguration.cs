﻿using FinancialSupport.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialSupport.Infra.Data.EntitiesConfiguration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.Property(t => t.Id).HasColumnName("Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Nome).HasColumnName("Nome").HasMaxLength(100).IsRequired();
            builder.Property(p => p.Foto).HasColumnName("Foto").HasMaxLength(100);
            builder.Property(p => p.Limite).HasColumnName("Limite").HasPrecision(10, 2);
            builder.Property(p => p.LimiteDisponivel).HasColumnName("LimiteDisponivel").HasPrecision(10, 2);
            builder.Property(p => p.DataCriacao);
            builder.Property(p => p.UsuarioCriacao).HasMaxLength(100);
            builder.Property(p => p.DataAlteracao);
            builder.Property(p => p.UsuarioAlteracao).HasMaxLength(100);
            builder.Property(p => p.Valendo);

            builder.HasMany(e => e.Emprestimos).WithOne(e => e.Usuario).HasForeignKey(c => c.IdUsuario);
        }
    }
}
