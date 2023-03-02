using FinancialSupport.Application.DTOs;

namespace FinancialSupport.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetUsuarios();
        Task<UsuarioDTO> GetById(int? id);
        Task<UsuarioDTO> GetHistoricoById(int? id);
        Task Add(UsuarioDTO usuarioDto);
        Task Update(UsuarioDTO usuarioDto);
        Task Update2(UsuarioDTO usuarioDto);
        Task Remove(UsuarioDTO usuarioDto);
        Task ApagaParcelasDeEmprestimo(int? idEmprestimo, string? operador);
        Task RecalculaLimite(int? cliente);
    }
}
