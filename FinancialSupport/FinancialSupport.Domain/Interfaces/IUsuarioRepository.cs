using FinancialSupport.Domain.Entities;

namespace FinancialSupport.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(int? id);
        Task<Usuario> GetUsuarioByNomeAsync(string? nome);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<Usuario> UpdateAsync(Usuario usuario);
        Task<Usuario> Update2Async(Usuario usuario);
        Task<Usuario> RemoveAsync(Usuario usuario);
        Task<Usuario> GetUsuarioHistoricoByIdAsync(int? id);
    }
}
