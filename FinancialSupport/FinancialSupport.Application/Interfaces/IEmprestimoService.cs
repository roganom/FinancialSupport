using FinancialSupport.Application.DTOs;

namespace FinancialSupport.Application.Interfaces
{
    public interface IEmprestimoService
    {
        Task<List<EmprestimoDTO>> GetEmprestimos();
        Task<EmprestimoDTO> GetEmprestimoById(int? id);
        Task<List<EmprestimoDTO>> GetEmprestimosByIdUsuario(int? idUsuario);
        Task Add(EmprestimoDTO emprestimoDto);
        Task Update(EmprestimoDTO emprestimoDto);
        Task Update2(EmprestimoDTO emprestimoDto);
        Task Remove(int? id);
        Task <KeyValuePair<int, string>> CriaNovoEmprestimo(int? idUsuario, decimal Valor, int parcelas, decimal juros, bool situacaoReal);
    }
}
