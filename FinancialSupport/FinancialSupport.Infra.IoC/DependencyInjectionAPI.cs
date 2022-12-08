using FinancialSupport.Application.Interfaces;
using FinancialSupport.Application.Mappings;
using FinancialSupport.Application.Services;
using FinancialSupport.Domain.Account;
using FinancialSupport.Domain.Interfaces;
using FinancialSupport.Infra.Data.Context;
using FinancialSupport.Infra.Data.Identity;
using FinancialSupport.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSupport.Infra.IoC
{
    public static class DependencyInjectionAPI
    {

        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
              b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
            services.AddScoped<IParcelaRepository, ParcelaRepository>();

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IEmprestimoService, EmprestimoService>();
            services.AddScoped<IParcelaService, ParcelaService>();

            services.AddScoped<IAutenticate, AutenticateService>();

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }
    }
}
