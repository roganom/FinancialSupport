using FinancialSupport.Domain.Entities;
using FinancialSupport.Infra.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialSupport.Infra.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);   // com Assembly pega tudo e não preciso instanciar cada Configuration
            //builder.ApplyConfiguration(new UsuarioConfiguration());
            //builder.ApplyConfiguration(new EmprestimoConfiguration());
            //builder.ApplyConfiguration(new ParcelaConfiguration());
         }
    }
}
