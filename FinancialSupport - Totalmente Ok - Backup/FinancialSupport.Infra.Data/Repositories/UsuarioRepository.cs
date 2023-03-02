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
        #region GETs: GetUsuariosAsync, GetUsuarioByIdAsync e GetUsuarioHistoricoByIdAsync
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            //return await _UsuarioContext.Usuarios.ToListAsync();
            return await _UsuarioContext.Usuarios.Where(i => i.Valendo == true).ToListAsync();
        }
        public async Task<Usuario> GetUsuarioByIdAsync(int? id)
        {
            //return await _UsuarioContext.Usuarios.FindAsync(id);
            return await _UsuarioContext.Usuarios.FirstOrDefaultAsync(i => i.Id == id && i.Valendo == true);
        }

        public async Task<Usuario> GetUsuarioHistoricoByIdAsync(int? id)
        {
            // return await _UsuarioContext.Usuarios.Include(i => i.Emprestimos).ThenInclude(i => i.Parcelas).FirstOrDefaultAsync(usu => usu.Id == id);
            return await _UsuarioContext.Usuarios.Include(i => i.Emprestimos.Where(j => j.Valendo == true))
                                             .ThenInclude(k => k.Parcelas.Where(l => l.Valendo == true))
                                             .FirstOrDefaultAsync(usu => usu.Id == id && usu.Valendo == true);
        }
        #endregion

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _UsuarioContext.Add(usuario);
            await _UsuarioContext.SaveChangesAsync();
            return usuario;
        }
        public async Task<Usuario> RemoveAsync(Usuario usuario)
        {

            _UsuarioContext.Usuarios.Remove(usuario);
            //_UsuarioContext.Usuarios.Update(usuario);
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
            usuarioAtual.DataAlteracao = usuario.DataAlteracao;
            usuarioAtual.UsuarioAlteracao = usuario.UsuarioAlteracao;
            usuarioAtual.Valendo = usuario.Valendo;
            usuarioAtual.DataCriacao = usuario.DataCriacao;
            usuarioAtual.UsuarioCriacao = usuario.UsuarioCriacao;
            usuarioAtual.Valendo = usuario.Valendo;
            usuarioAtual.Foto = usuario.Foto;
            usuarioAtual.Nome = usuario.Nome;
            usuarioAtual.Limite = usuario.Limite;
            //usuarioAtual.DDDTelefone1 = usuario.DDDTelefone1;
            //usuarioAtual.Telefone1 = usuario.Telefone1;
            //usuarioAtual.DDDTelefone2 = usuario.DDDTelefone2;
            //usuarioAtual.Telefone2= usuario.Telefone2;
            //usuarioAtual.Endereco = usuario.Endereco;
            //usuarioAtual.Empresa = usuario.Empresa; 

            await _UsuarioContext.SaveChangesAsync();
            return usuario;
        }
    }
}