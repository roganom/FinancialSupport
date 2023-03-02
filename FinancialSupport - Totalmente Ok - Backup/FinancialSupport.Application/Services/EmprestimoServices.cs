using AutoMapper;
using FinancialSupport.Application.DTOs;
using FinancialSupport.Application.Interfaces;
using FinancialSupport.Domain.Entities;
using FinancialSupport.Domain.Interfaces;

namespace FinancialSupport.Application.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private IEmprestimoRepository _emprestimoRepository;
        private IUsuarioRepository _usuarioRepository;
        private IParcelaRepository _parcelaRepository;
        private IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        public EmprestimoService(IMapper mapper, IEmprestimoRepository emprestimoRepository, IUsuarioRepository usuarioRepository, IParcelaRepository parcelaRepository, IUsuarioService usuarioService)
        {
            _emprestimoRepository = emprestimoRepository ?? throw new ArgumentNullException(nameof(emprestimoRepository));
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _parcelaRepository = parcelaRepository;
            _usuarioService = usuarioService;
        }

        #region GETs básicos GetEmprestimos, GetEmprestimoById e GetEmprestimosByIdUsuario
        public async Task<List<EmprestimoDTO>> GetEmprestimos()
        {
            var emprestimoEntity = await _emprestimoRepository.GetEmprestimosAsync();
            return _mapper.Map<List<EmprestimoDTO>>(emprestimoEntity);
        }
        public async Task<EmprestimoDTO> GetEmprestimoById(int? id)
        {
            var emprestimoEntity = await _emprestimoRepository.GetEmprestimoByIdAsync(id);
            return _mapper.Map<EmprestimoDTO>(emprestimoEntity);
        }
        public async Task<List<EmprestimoDTO>> GetEmprestimosByIdUsuario(int? idUsuario)
        {
            var emprestimoEntity = await _emprestimoRepository.GetEmprestimosByIdUsuarioAsync(idUsuario);
            return _mapper.Map<List<EmprestimoDTO>>(emprestimoEntity);
        }
        #endregion

        #region outras operações básicas Add, Upadate, Update2 e Remove
        public async Task Add(EmprestimoDTO emprestimoDto)
        {
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.CreateAsync(emprestimoEntity);
        }
        public async Task Update(EmprestimoDTO emprestimoDto)
        {
            emprestimoDto.DataAlteracao = DateTime.Now;
            emprestimoDto.Valendo = true;
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.UpdateAsync(emprestimoEntity);
        }
        public async Task Update2(EmprestimoDTO emprestimoDto)
        {
            emprestimoDto.DataAlteracao = DateTime.Now;
            emprestimoDto.Valendo = true;
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.Update2Async(emprestimoEntity);
        }

        public async Task Remove(EmprestimoDTO emprestimoDto)
        {
            emprestimoDto.DataAlteracao = DateTime.Now;
            emprestimoDto.Valendo = false;

            await _usuarioService.ApagaParcelasDeEmprestimo(emprestimoDto.Id, emprestimoDto.UsuarioAlteracao);
            
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.Update2Async(emprestimoEntity);
        }
        #endregion

        #region Altera e Deleta apenas na adminitração, pois é necessário recalcular o valor do limite
        public async Task UpdateAdm(EmprestimoDTO emprestimoDto)
        {
            emprestimoDto.DataAlteracao = DateTime.Now;
            emprestimoDto.Valendo = true;
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.UpdateAsync(emprestimoEntity);
            await RecalculaEmprestimo(emprestimoDto.Id, emprestimoDto.IdUsuario);
        }
        public async Task Update2Adm(EmprestimoDTO emprestimoDto)
        {
            emprestimoDto.DataAlteracao = DateTime.Now;
            emprestimoDto.Valendo = true;
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.Update2Async(emprestimoEntity);
            await RecalculaEmprestimo(emprestimoDto.Id, emprestimoDto.IdUsuario);
        }

        public async Task RemoveAdm(EmprestimoDTO emprestimoDto)
        {
            emprestimoDto.DataAlteracao = DateTime.Now;
            emprestimoDto.Valendo = false;

            await _usuarioService.ApagaParcelasDeEmprestimo(emprestimoDto.Id, emprestimoDto.UsuarioAlteracao);

            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.Update2Async(emprestimoEntity);
            await RecalculaEmprestimo(emprestimoDto.Id, emprestimoDto.IdUsuario);
        }
        #endregion

        #region CriaNovoEmprestimo
        public async Task <KeyValuePair<int, string>> CriaNovoEmprestimo(int? id, decimal valor, int parcelas, decimal juros, bool situacaoReal, string operador)
        {
            var emprestimoEntity = new Emprestimo();
            var usuarioEntity = new Usuario();
            var parcelaDto = new List<Parcela>();
            decimal valorParcela = 0;
            decimal valorTotal = 0;
            decimal limiteDisponivel = 0;

            emprestimoEntity.IdUsuario = id;
            emprestimoEntity.Valor = valor;
            emprestimoEntity.Data = DateTime.Today;
            emprestimoEntity.Ativo = true;
            emprestimoEntity.NumeroParcelas = parcelas;
            emprestimoEntity.UsuarioCriacao = operador;
            emprestimoEntity.DataCriacao = DateTime.Now;
            emprestimoEntity.Valendo = true;

            //calcula o valor da parcela, arredondando os centavos para cima
            valorParcela = (valor + ((valor * (decimal)parcelas * juros) / 100)) / (decimal)parcelas;
            valorParcela = Math.Ceiling(valorParcela);
            
            // determina o valor total do empréstimo este valor deverá ser abatido do limite disponível
            valorTotal = valorParcela * parcelas;

            usuarioEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);

            limiteDisponivel = (decimal)usuarioEntity.LimiteDisponivel;

            // vetifica se não é uma simulação
            if (situacaoReal)
            {
                // verifica se tem limite para a realização do empréstimo
                if (usuarioEntity.LimiteDisponivel >= valorTotal)
                {
                    for (int i = 0; i < parcelas; i++)
                    {
                        var parcelaEntity = new Parcela();
                        parcelaEntity.DataParcela = DateTime.Today.AddDays(i + 1);
                        parcelaEntity.ValorParcela = valorParcela;
                        parcelaEntity.Valendo = true;
                        parcelaEntity.DataCriacao = DateTime.Now;
                        parcelaEntity.UsuarioCriacao = operador;
                        parcelaDto.Add(parcelaEntity);
                    }

                    emprestimoEntity.Parcelas = parcelaDto;
                    await _emprestimoRepository.CreateAsync(emprestimoEntity);

                    //// atualiza o valor do limite (na tabela do usuário)
                    usuarioEntity.LimiteDisponivel = limiteDisponivel - valorTotal;

                    await _usuarioRepository.Update2Async(usuarioEntity);
                }
                else // caso o limite não seja suficiente para o empréstimo
                {
                    return new KeyValuePair<int, string>(3, "Empréstimo não realizado. Foi solicitado R$ " + valorTotal.ToString("N2") + " e o limite é de R$ " + limiteDisponivel.ToString("N2") + ".");
                }
                // retrno de empréstimo realizado com sucesso
                return new KeyValuePair<int, string>(1, "Empréstimo de R$ " + valorTotal.ToString("N2") + " (" + parcelas + " parcelas de R$ " + valorParcela.ToString("N2") + ") realizado com sucesso.");
            }
            else // retorno da simulação
                return new KeyValuePair<int, string>(2, "Simulação: " + parcelas + " parcelas de R$ " + valorParcela.ToString("N2") + " totalizando R$ " + valorTotal.ToString("N2") + ".");
        }
        #endregion

        public async Task RecalculaEmprestimo(int? emprestimo)
        {
            // caso teha alguma deleção ou alteração em algum emprétimo, chama RecalculaLimite(int cliente)
            var result = await GetEmprestimoById(emprestimo);

            await _usuarioService.RecalculaLimite(result.IdUsuario);
        }

        public async Task RecalculaEmprestimo(int? emprestimo, int? cliente)
        {
            // caso teha alguma deleção ou alteração em algum emprétimo, chama RecalculaLimite(int cliente)
            await _usuarioService.RecalculaLimite(cliente);
        }
    }
}