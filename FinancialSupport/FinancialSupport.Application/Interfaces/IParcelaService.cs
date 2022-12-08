using FinancialSupport.Application.DTOs;

namespace FinancialSupport.Application.Interfaces
{
    public interface IParcelaService
    {
        Task<List<ParcelaDTO>> GetParcelas(); 
        Task<ParcelaDTO> GetById(int? id);
        Task<ParcelaDTO> GetByIdEmprestimo(int? id);
        Task Add(ParcelaDTO parcelaDto);
        Task Update(ParcelaDTO parcelaDto);
        Task Update2(ParcelaDTO parcelaDto);
        Task Remove(int? id);
    }
}
