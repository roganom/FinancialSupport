using FinancialSupport.Domain.Entities;
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
            builder.Property(p => p.Limite).HasColumnName("Limite").HasPrecision(10, 2).IsRequired();
            builder.Property(p => p.LimiteDisponivel).HasColumnName("LimiteDisponivel").HasPrecision(10, 2).IsRequired();

            builder.HasMany(e => e.Emprestimos).WithOne(e => e.Usuario).HasForeignKey(c => c.IdUsuario);

            //TESTE###
            //builder.HasData(
            //    new Usuario(1, "Zenóbio", "path da foto1", 100, 100),
            //    new Usuario(2, "Zenóbia", "path da foto2", 200, 200),
            //    new Usuario(3, "Josefino", "path da foto3", 300, 300)
            //    );
        }
    }
}
