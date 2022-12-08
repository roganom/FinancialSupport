using FinancialSupport.Domain.Entities;
using FinancialSupport.Domain.Interfaces;
using FinancialSupport.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FinancialSupport.Infra.Data.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private ApplicationDbContext _EmprestimoContext;

        public EmprestimoRepository(ApplicationDbContext context)
        {
            _EmprestimoContext = context;
        }

        public async Task<Emprestimo> CreateAsync(Emprestimo emprestimo)
        {
            emprestimo.Data = emprestimo.Data == DateTime.Parse("1900-01-01") ? null : emprestimo.Data;

            _EmprestimoContext.Add(emprestimo);
            await _EmprestimoContext.SaveChangesAsync();
            return emprestimo;
        }
        public async Task<Emprestimo> GetEmprestimoByIdAsync(int? id)
        {
            Emprestimo? emprestimo = await _EmprestimoContext.Emprestimos.FindAsync(id);
            return emprestimo;
        }
        public async Task<List<Emprestimo>> GetEmprestimosAtivosAsync()
        {
            return await _EmprestimoContext.Emprestimos.Where(p => p.Ativo).ToListAsync();
        }
        public async Task<List<Emprestimo>> GetEmprestimosByIdUsuarioAsync(int? id)
        {
            //eager loading
            return await _EmprestimoContext.Emprestimos.Where(p => p.IdUsuario == id).ToListAsync();
        }

        public async Task<List<Emprestimo>> GetEmprestimosAsync()
        {
            return await _EmprestimoContext.Emprestimos.ToListAsync();
        }

        public async Task<Emprestimo> RemoveAsync(Emprestimo emprestimo)
        {
            _EmprestimoContext.Remove(emprestimo);
            await _EmprestimoContext.SaveChangesAsync();
            return emprestimo;
        }

        public async Task<Emprestimo> UpdateAsync(Emprestimo emprestimo)
        {
            emprestimo.Data = emprestimo.Data == DateTime.Parse("1900-01-01") ? null : emprestimo.Data;

            _EmprestimoContext.Update(emprestimo);
            await _EmprestimoContext.SaveChangesAsync();
            return emprestimo;
        }

        public async Task<Emprestimo> Update2Async(Emprestimo emprestimo)
        {
            var emprestimoAtual = await this.GetEmprestimoByIdAsync(emprestimo.Id);
            emprestimoAtual.Ativo = emprestimo.Ativo;

            await _EmprestimoContext.SaveChangesAsync();
            return emprestimo;
        }
    }
}