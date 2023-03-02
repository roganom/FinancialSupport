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

        #region GETs
        public async Task<Emprestimo> GetEmprestimoByIdAsync(int? id)
        {
            return await _EmprestimoContext.Emprestimos.FirstOrDefaultAsync(i => i.Id == id && i.Valendo == true);

        }
        public async Task<List<Emprestimo>> GetEmprestimosAtivosAsync()
        {
            return await _EmprestimoContext.Emprestimos.Where(p => p.Ativo && p.Valendo == true).ToListAsync();
        }
        public async Task<List<Emprestimo>> GetEmprestimosByIdUsuarioAsync(int? id)
        {
            //eager loading
            return await _EmprestimoContext.Emprestimos.Where(p => p.IdUsuario == id && p.Valendo == true).ToListAsync();
        }

        public async Task<List<Emprestimo>> GetEmprestimosAsync()
        {
            return await _EmprestimoContext.Emprestimos.Where(p => p.Valendo == true).ToListAsync();
        }
        #endregion
        public async Task<Emprestimo> CreateAsync(Emprestimo emprestimo)
        {
            emprestimo.Data = emprestimo.Data == DateTime.Parse("1900-01-01") ? null : emprestimo.Data;

            _EmprestimoContext.Add(emprestimo);
            await _EmprestimoContext.SaveChangesAsync();
            return emprestimo;
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
            emprestimoAtual.Valendo = emprestimo.Valendo;
            emprestimoAtual.DataAlteracao = emprestimo.DataAlteracao;
            emprestimoAtual.UsuarioAlteracao = emprestimo.UsuarioAlteracao;
            emprestimoAtual.IdUsuario = emprestimo.IdUsuario;
            emprestimoAtual.Valor = emprestimo.Valor;
            emprestimoAtual.Data = emprestimo.Data;
            emprestimoAtual.NumeroParcelas = emprestimo.NumeroParcelas;
            emprestimoAtual.DataCriacao = emprestimo.DataCriacao;
            emprestimoAtual.UsuarioCriacao = emprestimo.UsuarioCriacao;

            await _EmprestimoContext.SaveChangesAsync();
            return emprestimo;
        }
    }
}