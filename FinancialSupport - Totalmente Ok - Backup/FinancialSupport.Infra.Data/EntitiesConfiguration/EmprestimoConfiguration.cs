using FinancialSupport.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FinancialSupport.Infra.Data.EntitiesConfiguration
{
    public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
    {
        public void Configure(EntityTypeBuilder<Emprestimo> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.IdUsuario);
            builder.Property(p => p.Valor).HasPrecision(10, 2).IsRequired();
            builder.Property(p => p.Data).IsRequired();
            builder.Property(p => p.Ativo);
            builder.Property(p => p.NumeroParcelas).IsRequired();
            builder.Property(p => p.DataCriacao);
            builder.Property(p => p.UsuarioCriacao).HasMaxLength(100);
            builder.Property(p => p.DataAlteracao);
            builder.Property(p => p.UsuarioAlteracao).HasMaxLength(100);
            builder.Property(p => p.Valendo);

            builder.HasMany(e => e.Parcelas).WithOne(e => e.Emprestimo).HasForeignKey(e => e.IdEmprestimo);
        }
    }
}
