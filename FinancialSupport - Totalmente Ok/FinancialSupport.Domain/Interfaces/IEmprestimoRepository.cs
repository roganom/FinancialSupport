using FinancialSupport.Domain.Entities;

namespace FinancialSupport.Domain.Interfaces
{
    public interface IEmprestimoRepository
    {
        Task<List<Emprestimo>> GetEmprestimosAsync();
        Task<List<Emprestimo>> GetEmprestimosAtivosAsync();
        Task<Emprestimo> GetEmprestimoByIdAsync(int? id);
        Task<List<Emprestimo>> GetEmprestimosByIdUsuarioAsync(int? id);
        Task<Emprestimo> CreateAsync(Emprestimo emprestimo);
        Task<Emprestimo> UpdateAsync(Emprestimo emprestimo);
        Task<Emprestimo> Update2Async(Emprestimo emprestimo);
        Task<Emprestimo> RemoveAsync(Emprestimo emprestimo);
    }
}
