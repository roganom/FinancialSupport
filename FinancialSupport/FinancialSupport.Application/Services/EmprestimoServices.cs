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

        private readonly IMapper _mapper;
        public EmprestimoService(IMapper mapper, IEmprestimoRepository emprestimoRepository, IUsuarioRepository usuarioRepository)
        {
            _emprestimoRepository = emprestimoRepository ?? throw new ArgumentNullException(nameof(emprestimoRepository));
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }
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
        public async Task Update(EmprestimoDTO emprestimoDto)
        {
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.UpdateAsync(emprestimoEntity);
        }
        public async Task Update2(EmprestimoDTO emprestimoDto)
        {
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.Update2Async(emprestimoEntity);
        }
        public async Task Add(EmprestimoDTO emprestimoDto)
        {
            var emprestimoEntity = _mapper.Map<Emprestimo>(emprestimoDto);
            await _emprestimoRepository.CreateAsync(emprestimoEntity);
        }
        public async Task Remove(int? id)
        {
            var emprestimoEntity = _emprestimoRepository.GetEmprestimoByIdAsync(id).Result;
            await _emprestimoRepository.RemoveAsync(emprestimoEntity);
        }
        public async Task <KeyValuePair<int, string>> CriaNovoEmprestimo(int? id, decimal valor, int parcelas, decimal juros, bool situacaoReal)
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

            valorParcela = (valor + ((valor * (decimal)parcelas * juros) / 100)) / (decimal)parcelas;
            valorParcela = Math.Ceiling(valorParcela);
            
            // verifica valor do empréstio e do limite dispnível
            valorTotal = valorParcela * parcelas;

            usuarioEntity = await _usuarioRepository.GetUsuarioByIdAsync(id);

            limiteDisponivel = usuarioEntity.LimiteDisponivel;

            if (situacaoReal)
            {
                if (usuarioEntity.LimiteDisponivel >= valorTotal)
                {
                    for (int i = 0; i < parcelas; i++)
                    {
                        var parcelaEntity = new Parcela();
                        parcelaEntity.DataParcela = DateTime.Today.AddDays(i + 1);
                        parcelaEntity.ValorParcela = valorParcela;
                        parcelaDto.Add(parcelaEntity);
                    }

                    emprestimoEntity.Parcelas = parcelaDto;

                    await _emprestimoRepository.CreateAsync(emprestimoEntity);

                    //// atualiza o valor do limite
                    usuarioEntity.LimiteDisponivel = usuarioEntity.LimiteDisponivel - valorTotal;
                    await _usuarioRepository.Update2Async(usuarioEntity);
                }
                else
                {
                    return new KeyValuePair<int, string>(3, "Empréstimo não realizado - solicitado foi " + valorTotal.ToString("N2") + " e o limite é " + limiteDisponivel.ToString("N2") + ".");
                }

                return new KeyValuePair<int, string>(1, "Empréstimo de R$ " + valorTotal.ToString("N2") + " realizado com sucesso.");
            }
            else
                return new KeyValuePair<int, string>(2, "Simulação: R$ " + valorTotal.ToString("N2") + " em " + parcelas + " parcelas de R$ " + valorParcela.ToString("N2") + ".");
        }
    }
}

