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

            builder.HasMany(e => e.Parcelas).WithOne(e => e.Emprestimo).HasForeignKey(e => e.IdEmprestimo);

            //TESTE###
            //builder.HasData(
            //new Emprestimo(1, 1, 10, DateTime.Parse("1971-11-04"), true, 60),
            //new Emprestimo(2, 2, 20, DateTime.Parse("1997-07-20"), true, 60),
            //new Emprestimo(3, 3, 30, DateTime.Parse("1978-02-20"), true, 60)
            //);
        }
    }
}
