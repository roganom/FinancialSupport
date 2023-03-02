using FinancialSupport.Application.DTOs;

namespace FinancialSupport.Application.Interfaces
{
    public interface IParcelaService
    {
        Task<List<ParcelaDTO>> GetParcelas(); 
        Task<ParcelaDTO> GetById(int? id);
        Task<List<ParcelaDTO>> GetByIdEmprestimo(int? id);
        Task Add(ParcelaDTO parcelaDto);
        Task Update(ParcelaDTO parcelaDto);
        Task Update2(ParcelaDTO parcelaDto);
        Task Remove(ParcelaDTO parcelaDto);
        Task UpdateAdm(ParcelaDTO parcelaDto);
        Task Update2Adm(ParcelaDTO parcelaDto);
        Task RemoveAdm(ParcelaDTO parcelaDto);
        Task RecalculaParcela(int? emprestimo);
    }
}
