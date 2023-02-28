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
        private IEmprestimoRepository _emprestimoRepository;
        private IParcelaRepository _parcelaRepository;
        private readonly IMapper _mapper;   
        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper, IEmprestimoRepository emprestimoRepository, IParcelaRepository parcelaRepository)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _emprestimoRepository = emprestimoRepository;
            _parcelaRepository = parcelaRepository;
        }
        #region GETs: GetById, GetUsuarios e GetHistoricoById
        public async Task<UsuarioDTO> GetById(int? id)
        {
            var usuariosEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);
            return _mapper.Map<UsuarioDTO>(usuariosEntity);
        }
        public async Task<List<UsuarioDTO>> GetUsuarios()
        {
            var usuariosEntity = await _usuarioRepository.GetUsuariosAsync();
            return _mapper.Map<List<UsuarioDTO>>(usuariosEntity);
        }
        public async Task<UsuarioDTO> GetHistoricoById(int? id)
        {
            var usuariosEntity = await _usuarioRepository.GetUsuarioHistoricoByIdAsync(id);
            return _mapper.Map<UsuarioDTO>(usuariosEntity);
        }
        #endregion

        #region Add, Update, Update2 e Remove
        public async Task Add(UsuarioDTO usuarioDto)
        {
            usuarioDto.DataCriacao = DateTime.Now;
            usuarioDto.Valendo = true;
            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.CreateAsync(usuarioEntity);
        }
        public async Task Update(UsuarioDTO usuarioDto)
        {
            usuarioDto.DataAlteracao = DateTime.Now;
            usuarioDto.Valendo = true;
            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.UpdateAsync(usuarioEntity);
        }
        public async Task Update2(UsuarioDTO usuarioDto)
        {
            usuarioDto.DataAlteracao = DateTime.Now;
            usuarioDto.Valendo = true;
            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.Update2Async(usuarioEntity);
        }
        public async Task Remove(UsuarioDTO usuarioDto)
        {
            usuarioDto.DataAlteracao = DateTime.Now;
            usuarioDto.Valendo = false;

            // retorna a lista de emprestimos deste cliente(usuário)
            var emprestimoEntity = await _emprestimoRepository.GetEmprestimosByIdUsuarioAsync(usuarioDto.Id); 

            foreach (var emp in emprestimoEntity)
            {
                // "apaga"(update2) todas as parcelas do empréstimo do loop
                await ApagaParcelasDeEmprestimo(emp.Id, emp.UsuarioAlteracao);

                emp.Valendo = false;
                emp.UsuarioAlteracao = usuarioDto.UsuarioAlteracao;
                emp.DataAlteracao = DateTime.Now;

                await _emprestimoRepository.Update2Async(emp);
            }

            var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            await _usuarioRepository.Update2Async(usuarioEntity);

            // apaga apenas usuários, sem empréstimos ou parcelas
            //var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
            //await _usuarioRepository.Update2Async(usuarioEntity);
        }
        #endregion

        #region  ApagaParcelasDeEmprestimo
        public async Task ApagaParcelasDeEmprestimo(int? idEmprestimo, string? operador)
        {
            var parcelaEntity = new List<Parcela>();
            parcelaEntity = await _parcelaRepository.GetParcelasByIdEmprestimoAsync(idEmprestimo);

            foreach (var item in parcelaEntity)
            {
                item.Valendo = false;
                item.UsuarioAlteracao = operador;
                item.DataAlteracao = DateTime.Now;

                await _parcelaRepository.Update2Async(item);
            }
        }
        #endregion
        public async Task RecalculaLimite(int? cliente)
        {
            //Soma todas as parcelas não quitadas de todos empréstios do cliente
            //Pega o valor do limite total
            //Novo limite disponível é a soma (acima) subtraida do Limite Total 
            if (cliente != null)
            {
                decimal emAberto = 0;
                decimal naoQuitada = 0;
                var usuarioDto = await GetHistoricoById(cliente);

                if (usuarioDto != null)
                {
                    foreach (var emp in usuarioDto.Emprestimos)
                    {
                        foreach (var parc in emp.Parcelas)
                        {
                            // se parcelas não quitadas de todos empréstios do cliente
                            if (parc.ValorParcela == null) parc.ValorParcela = 0;
                            if (parc.ValorPagamento == null) parc.ValorPagamento = 0;

                            naoQuitada = parc.ValorParcela - (decimal)parc.ValorPagamento;

                            if (naoQuitada > 0)
                                emAberto += naoQuitada;
                        }
                    }

                    //var usuarioDto = await GetById(cliente);
                    usuarioDto.LimiteDisponivel = usuarioDto.Limite - emAberto;
                    var usuarioEntity = _mapper.Map<Usuario>(usuarioDto);
                    await _usuarioRepository.Update2Async(usuarioEntity);
                }
            }
        }
    }
}
