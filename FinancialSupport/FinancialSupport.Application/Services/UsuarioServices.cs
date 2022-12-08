using AutoMapper;
using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.Domain.Entities;
using FinancialSupport.Domain.Interfaces;

namespace FinancialSupport.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;   
        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task Add(UsuarioDTO usuarioDto)
        {
            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.CreateAsync(usuarioEntity);
        }
        public async Task<UsuarioDTO> GetById(int? id)
        {
            var usuariosEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);
            return _mapper.Map<UsuarioDTO>(usuariosEntity);
        }
        public async Task<UsuarioDTO> GetByNome(string? nome)
        {
            var usuariosEntity = await _usuarioRepository.GetUsuarioByNomeAsync(nome);
            return _mapper.Map<UsuarioDTO>(usuariosEntity);
        }
        public async Task<List<UsuarioDTO>> GetUsuarios()
        {
            var usuariosEntity = await _usuarioRepository.GetUsuariosAsync();
            return _mapper.Map<List<UsuarioDTO>>(usuariosEntity);
        }
        public async Task Remove(int? id)
        {
            var usuariosEntity = _usuarioRepository.GetUsuarioByIdAsync(id).Result;
            await _usuarioRepository.RemoveAsync(usuariosEntity);
        }
        public async Task<UsuarioDTO> GetHistoricoById(int? id)
        {
            var usuariosEntity = await _usuarioRepository.GetUsuarioHistoricoByIdAsync(id);
            return _mapper.Map<UsuarioDTO>(usuariosEntity);
        }
        public async Task Update(UsuarioDTO usuarioDto)
        {
            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.UpdateAsync(usuarioEntity);
        }
        public async Task Update2(UsuarioDTO usuarioDto)
        {
            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.Update2Async(usuarioEntity);
        }
    }
}
