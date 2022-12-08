using FinancialSupport.Domain.Entities;
using FinancialSupport.Domain.Interfaces;
using FinancialSupport.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace FinancialSupport.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private ApplicationDbContext _UsuarioContext;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _UsuarioContext = context;
        }
        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _UsuarioContext.Add(usuario);
            await _UsuarioContext.SaveChangesAsync();
            return usuario;
        }
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _UsuarioContext.Usuarios.ToListAsync();
        }
        public async Task<Usuario> GetUsuarioByIdAsync(int? id)
        {
            return await _UsuarioContext.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> GetUsuarioHistoricoByIdAsync(int? id)
        {
            //return await _UsuarioContext.Usuarios.Include(i => i.Emprestimos).FirstOrDefaultAsync(usu => usu.Id == id);
            return await _UsuarioContext.Usuarios.Include(i => i.Emprestimos).ThenInclude(i => i.Parcelas).FirstOrDefaultAsync(usu => usu.Id == id);
        }

        public async Task<Usuario> GetUsuarioByNomeAsync(string? nome)
        {
            return await _UsuarioContext.Usuarios.FindAsync(nome);
        }
        public async Task<Usuario> RemoveAsync(Usuario usuario)
        {
            _UsuarioContext.Usuarios.Remove(usuario);
            await _UsuarioContext.SaveChangesAsync();
            return usuario;
        }
        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
           _UsuarioContext.Usuarios.Update(usuario);
            await _UsuarioContext.SaveChangesAsync();
            return usuario;
        }
        public async Task<Usuario> Update2Async(Usuario usuario)
        {
            var usuarioAtual = await this.GetUsuarioByIdAsync(usuario.Id);
            usuarioAtual.LimiteDisponivel = usuario.LimiteDisponivel;

            await _UsuarioContext.SaveChangesAsync();
            return usuario;
        }
    }
}