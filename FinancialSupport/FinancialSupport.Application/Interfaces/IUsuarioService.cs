using FinancialSupport.Application.DTOs;

namespace FinancialSupport.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetUsuarios();
        Task<UsuarioDTO> GetById(int? id);
        Task<UsuarioDTO> GetByNome(string? nome);
        Task Add(UsuarioDTO usuarioDto);
        Task Update(UsuarioDTO usuarioDto);
        Task Update2(UsuarioDTO usuarioDto);
        Task Remove(int? id);
        Task<UsuarioDTO> GetHistoricoById(int? id);
    }
}
