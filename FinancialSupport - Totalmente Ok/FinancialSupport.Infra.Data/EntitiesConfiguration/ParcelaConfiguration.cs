using FinancialSupport.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialSupport.Infra.Data.EntitiesConfiguration
{
    public class ParcelaConfiguration : IEntityTypeConfiguration<Parcela>
    {
        public void Configure(EntityTypeBuilder<Parcela> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.IdEmprestimo).IsRequired();
            builder.Property(p => p.DataParcela).IsRequired();
            builder.Property(p => p.ValorParcela).HasPrecision(10, 2).IsRequired();
            builder.Property(p => p.DataPagamento);
            builder.Property(p => p.ValorPagamento).HasPrecision(10, 2);
            builder.Property(p => p.DataCriacao);
            builder.Property(p => p.UsuarioCriacao).HasMaxLength(100);
            builder.Property(p => p.DataAlteracao);
            builder.Property(p => p.UsuarioAlteracao).HasMaxLength(100);
            builder.Property(p => p.Valendo);

            builder.HasOne(e => e.Emprestimo).WithMany(e => e.Parcelas).HasForeignKey(e => e.IdEmprestimo);

            //TESTE para usar MIGRATIONS ###
            //builder.HasData(
            //    new Parcela(1, 1, DateTime.Parse("2021-10-01"), 100, DateTime.Parse("2021-10-01"), 100),
            //    new Parcela(2, 1, DateTime.Parse("2021-10-02"), 100, DateTime.Parse("2021-10-02"), 100),
            //    new Parcela(3, 2, DateTime.Parse("2021-12-01"), 100, DateTime.Parse("2021-12-01"), 0),
            //    new Parcela(4, 1, DateTime.Parse("2021-10-03"), 100, DateTime.Parse("2021-10-03"), 100),
            //    new Parcela(5, 1, DateTime.Parse("2021-10-04"), 100, DateTime.Parse("2021-10-04"), 100),
            //    new Parcela(6, 1, DateTime.Parse("2021-10-05"), 100, DateTime.Parse("2021-10-05"), 100)
            //    );
        }
    }
}