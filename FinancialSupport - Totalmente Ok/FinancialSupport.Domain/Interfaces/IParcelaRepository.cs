using FinancialSupport.Domain.Entities;

namespace FinancialSupport.Domain.Interfaces
{
    public interface IParcelaRepository
    {
        Task<List<Parcela>> GetParcelas();
        Task<Parcela> GetParcelaByIdAsync(int? id);
        //Task<Parcela> GetParcelaByIdEmprestimoAsync(int? id);
        Task<List<Parcela>> GetParcelasByIdEmprestimoAsync(int? id);
        Task<Parcela> CreateAsync(Parcela parcela);
        Task<Parcela> UpdateAsync(Parcela parcela);
        Task<Parcela> Update2Async(Parcela parcela);
        Task<Parcela> RemoveAsync(Parcela parcela);
    }
}
