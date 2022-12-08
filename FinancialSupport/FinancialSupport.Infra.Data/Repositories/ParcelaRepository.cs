using FinancialSupport.Domain.Entities;
using FinancialSupport.Domain.Interfaces;
using FinancialSupport.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FinancialSupport.Infra.Data.Repositories
{
    public class ParcelaRepository : IParcelaRepository
    {
        private ApplicationDbContext _ParcelaContext;
        public ParcelaRepository(ApplicationDbContext context)
        {
            _ParcelaContext = context;
        }
        public async Task<Parcela> CreateAsync(Parcela parcela)
        {

            parcela.DataPagamento = parcela.DataPagamento == DateTime.Parse("1900-01-01") ? null : parcela.DataPagamento;

            _ParcelaContext.Add(parcela);
            await _ParcelaContext.SaveChangesAsync();
            return parcela;
        }
        public async Task<List<Parcela>> GetParcelas()
        {
            return await _ParcelaContext.Parcelas.ToListAsync();
        }
        public async Task<Parcela> GetParcelaByIdAsync(int? id)
        {
            return await _ParcelaContext.Parcelas.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Parcela>> GetParcelaByIdEmprestimoAsync(int? id)
        {
            return await _ParcelaContext.Parcelas.Where(p => p.IdEmprestimo == id).ToListAsync();
        }
        public async Task<Parcela> RemoveAsync(Parcela parcela)
        {
            _ParcelaContext.Parcelas.Remove(parcela);
            await _ParcelaContext.SaveChangesAsync();
            return parcela;
        }
        public async Task<Parcela> UpdateAsync(Parcela parcela)
        {
            parcela.DataPagamento = parcela.DataPagamento == DateTime.Parse("1900-01-01") ? null : parcela.DataPagamento;

            _ParcelaContext.Parcelas.Update(parcela);
            await _ParcelaContext.SaveChangesAsync();
            return parcela;
        }

        public async Task<Parcela> Update2Async(Parcela parcela)
        {
            var parcelaAtual = await this.GetParcelaByIdAsync(parcela.Id);

            parcelaAtual.DataPagamento = parcela.DataPagamento;
            parcelaAtual.ValorPagamento = parcela.ValorPagamento;

            await _ParcelaContext.SaveChangesAsync();
            return parcela;
        }
    }
}
